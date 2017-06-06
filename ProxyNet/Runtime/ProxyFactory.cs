namespace ProxyNet.Runtime
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    public static class ProxyFactory
    {
        public static Type BuildType(
            Type itf,
            string assemblyName,
            string moduleName,
            string typeName,
            AssemblyBuilderAccess access)
        {
            var methods = GetMethods(itf);

            var attributes = GetAttributes(itf);

            var modBldr = CreateModule(itf, assemblyName, moduleName, typeName, access);

            var typeBldr = modBldr.DefineType(
                typeName,
                TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public,
                typeof(ProxyClient),
                new[] { itf });

            BuildConstructor(typeBldr, typeof(ProxyClient));
            BuildAttributes(typeBldr, attributes);
            BuildMethods(typeBldr, methods);

            return typeBldr.CreateType();
        }

        private static ModuleBuilder CreateModule(
            Type itf,
            string assemblyName,
            string moduleName,
            string typeName,
            AssemblyBuilderAccess access)
        {
            var assName = new AssemblyName();
            assName.Name = assemblyName;

            if (access == AssemblyBuilderAccess.RunAndSave)
                assName.Version = itf.Assembly.GetName().Version;

            var assBldr = AppDomain.CurrentDomain.DefineDynamicAssembly(assName, access);

            var modBldr = (access == AssemblyBuilderAccess.Run
                ? assBldr.DefineDynamicModule(assName.Name)
                : assBldr.DefineDynamicModule(assName.Name, moduleName));

            return modBldr;
        }

        //Rever método, herança de interfaces
        private static MethodInfo[] GetMethods(Type type)
        {
            var methods = type.GetMethods();

            return methods;
        }

        private static CustomAttributeData[] GetAttributes(Type type)
        {
            var attributes = type.GetCustomAttributesData().ToArray();

            return attributes;
        }

        private static void BuildConstructor(
           TypeBuilder typeBldr,
           Type baseType)
        {
            var ctorBldr = typeBldr.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName | MethodAttributes.HideBySig,
                CallingConventions.Standard,
                Type.EmptyTypes);

            var ilgen = ctorBldr.GetILGenerator();
            //  Call the base constructor.
            ilgen.Emit(OpCodes.Ldarg_0);
            var ctorInfo = baseType.GetConstructor(Type.EmptyTypes);
            ilgen.Emit(OpCodes.Call, ctorInfo);
            ilgen.Emit(OpCodes.Ret);
        }

        private static void BuildMethods(TypeBuilder typeBldr, MethodInfo[] methods)
        {
            foreach (MethodInfo method in methods)
            {
                var parameters = method.GetParameters();
                var paramNames = parameters.Select(a => a.Name).ToArray();
                var paramTypes = parameters.Select(a => a.ParameterType).ToArray();
                var attributes = method.GetCustomAttributesData().ToArray();

                BuildMethod(typeBldr, method.Name, paramNames, paramTypes, method.ReturnType, attributes);
            }
        }

        private static void BuildMethod(TypeBuilder typeBldr, string methodName, string[] paramNames, Type[] paramTypes, Type returnType, CustomAttributeData[] attributes)
        {
            var mthdBldr = typeBldr.DefineMethod(
               methodName,
               MethodAttributes.Public | MethodAttributes.Virtual,
               returnType,
               paramTypes);

            BuildAttributes(mthdBldr, attributes);

            for (var i = 0; i < paramNames.Length; i++)
            {
                mthdBldr.DefineParameter(i + 1, ParameterAttributes.In, paramNames[i]);
            }

            var ilgen = mthdBldr.GetILGenerator();

            LocalBuilder retVal = null;
            LocalBuilder tempRetVal = null;

            if (typeof(void) != returnType)
            {
                tempRetVal = ilgen.DeclareLocal(typeof(object));
                retVal = ilgen.DeclareLocal(returnType);
            }

            var argValues = ilgen.DeclareLocal(typeof(object[]));
            ilgen.Emit(OpCodes.Ldc_I4, paramTypes.Length);
            ilgen.Emit(OpCodes.Newarr, typeof(object));
            ilgen.Emit(OpCodes.Stloc, argValues);

            for (var argLoad = 0; argLoad < paramTypes.Length; argLoad++)
            {
                ilgen.Emit(OpCodes.Ldloc, argValues);
                ilgen.Emit(OpCodes.Ldc_I4, argLoad);
                ilgen.Emit(OpCodes.Ldarg, argLoad + 1);
                if (paramTypes[argLoad].IsValueType)
                    ilgen.Emit(OpCodes.Box, paramTypes[argLoad]);
                ilgen.Emit(OpCodes.Stelem_Ref);
            }

            var invokeTypes = new[] { typeof(MethodInfo), typeof(object[]) };
            var invokeMethod = typeof(ProxyClient).GetMethod("Invoke", invokeTypes);

            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, typeof(MethodBase).GetMethod("GetCurrentMethod"));
            ilgen.Emit(OpCodes.Castclass, typeof(MethodInfo));
            ilgen.Emit(OpCodes.Ldloc, argValues);
            ilgen.Emit(OpCodes.Call, invokeMethod);


            if (typeof(void) != returnType)
            {
                // if return value is null, don't cast it to required type
                var retIsNull = ilgen.DefineLabel();
                ilgen.Emit(OpCodes.Stloc, tempRetVal);
                ilgen.Emit(OpCodes.Ldloc, tempRetVal);
                ilgen.Emit(OpCodes.Brfalse, retIsNull);
                ilgen.Emit(OpCodes.Ldloc, tempRetVal);

                if (returnType.IsValueType)
                {
                    ilgen.Emit(OpCodes.Unbox, returnType);
                    ilgen.Emit(OpCodes.Ldobj, returnType);
                }
                else
                    ilgen.Emit(OpCodes.Castclass, returnType);

                ilgen.Emit(OpCodes.Stloc, retVal);
                ilgen.MarkLabel(retIsNull);
                ilgen.Emit(OpCodes.Ldloc, retVal);
            }
            else
                ilgen.Emit(OpCodes.Pop);
            ilgen.Emit(OpCodes.Ret);
        }

        private static void BuildAttributes(
           TypeBuilder typeBldr,
           CustomAttributeData[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var cab = new CustomAttributeBuilder(attribute.Constructor, attribute.ConstructorArguments.Select(a => a.Value).ToArray());

                typeBldr.SetCustomAttribute(cab);
            }
        }

        private static void BuildAttributes(
           MethodBuilder methodBldr,
           CustomAttributeData[] attributes)
        {
            foreach (var attribute in attributes)
            {
                var cab = new CustomAttributeBuilder(attribute.Constructor, attribute.ConstructorArguments.Select(a => a.Value).ToArray());

                methodBldr.SetCustomAttribute(cab);
            }
        }
    }
}
