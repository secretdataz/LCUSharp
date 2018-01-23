using EasyHttp.Http;
using LCUSharp.DataObjects;

namespace LCUSharp
{
    public delegate void LeagueClosedHandler();

    public interface ILeagueClient
    {
        /**
         * Executed when League of Legends client watched by this process is closed.
         */
        event LeagueClosedHandler LeagueClosed;
        HttpResponse MakeApiRequest(HttpMethod method, string endpoint, object data = null, string contentType = HttpContentTypes.ApplicationJson);
        RuneManager GetRuneManager();
    }

    public enum HttpMethod
    {
        Get,
        Post,
        Put,
        Delete
    }
}
