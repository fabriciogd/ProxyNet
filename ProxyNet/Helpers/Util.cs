namespace ProxyNet.Helpers
{
    using System.IO;

    public class Util
    {
        static public void CopyStream(Stream src, Stream dst)
        {
            byte[] buff = new byte[4096];
            while (true)
            {
                int read = src.Read(buff, 0, 4096);
                if (read == 0)
                    break;
                dst.Write(buff, 0, read);
            }
        }
    }
}
