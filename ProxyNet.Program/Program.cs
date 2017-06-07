using ProxyNet.Attributes;
using ProxyNet.Enum;

namespace ProxyNet.Program
{
    class Program
    {
        static void Main(string[] args)
        {
            ITesteProxy proxy = Proxy.Create<ITesteProxy>();

            object addedPost = proxy.AddPost(new { title = "Adicionar" });

            object[] posts = proxy.ListPosts();

            object post = proxy.GetPost(2);

            object[] comments = proxy.GetComments(2);

            proxy.DeletePost(2);
        }
    }

    [ProxyUrl("http://jsonplaceholder.typicode.com")]
    public interface ITesteProxy : IProxy
    {
        [ProxyUrl("/posts")]
        [ProxyMethod(Method.POST)]
        [ProxyContentType("application/json")]
        object AddPost(object post);

        [ProxyUrl("/posts")]
        [ProxyMethod(Method.GET)]
        object[] ListPosts();

        [ProxyUrl("/posts?postId={postId}")]
        [ProxyMethod(Method.GET)]
        object GetPost(int postId);

        [ProxyUrl("/posts/{postId}/comments")]
        [ProxyMethod(Method.GET)]
        object[] GetComments(int postId);

        [ProxyUrl("/posts/{postId}")]
        [ProxyMethod(Method.DELETE)]
        void DeletePost(int postId);
    }
}
