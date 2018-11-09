using System.Net.Http;
using System.Threading.Tasks;

namespace LCUSharp
{
    public delegate void LeagueClosedHandler();

    public interface ILeagueClient
    {
        /**
         * Executed when League of Legends client watched by this process is closed.
         */
        event LeagueClosedHandler LeagueClosed;
        RuneManager GetRuneManager();
        HttpClient GetHttpClient();
        Task<HttpResponseMessage> MakeApiRequest(HttpMethod method, string endpoint, object data = null);
        Task<T> MakeApiRequestAs<T>(HttpMethod method, string endpoint, object data = null);
    }

    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
}
