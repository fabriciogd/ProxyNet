using ProxyNet.Attributes;
using ProxyNet.Enum;

namespace ProxyNet.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ITesteProxy proxy = Proxy.Create<ITesteProxy>();

            object adicionado = proxy.Adicionar(new { title = "Adicionar" });

            object[] lista = proxy.Listar();

            object item = proxy.Obter(2);

            object[] comentarios = proxy.Comments(2);
        }
    }

    [ProxyUrl("http://jsonplaceholder.typicode.com")]
    public interface ITesteProxy : IProxy
    {
        [ProxyUrl("/posts")]
        [ProxyMethod(Method.POST)]
        [ProxyContentType("application/json")]
        object Adicionar(object post);

        [ProxyUrl("/posts")]
        [ProxyMethod(Method.GET)]
        object[] Listar();

        [ProxyUrl("/posts")]
        [ProxyMethod(Method.GET)]
        [ProxyParameterInfo("postId")]
        object Obter(int postId);

        [ProxyUrl("/posts/{postId}/comments")]
        [ProxyMethod(Method.GET)]
        [ProxyParameterInfo("postId")]
        object[] Comments(int postId);
    }
}
