using LCUSharp.DataObjects;
using System.Net;

namespace LCUSharp
{
    public class RuneManager
    {
        ILeagueClient League;
        const string endpointRoot = "lol-perks/v1/";

        public RuneManager(ILeagueClient league)
        {
            League = league;
        }

        public RunePage GetCurrentRunePage()
        {
            return League.MakeApiRequestAs<RunePage>(HttpMethod.Get, endpointRoot + "currentpage").Result;
        }

        public RunePage[] GetRunePages()
        {
            return League.MakeApiRequestAs<RunePage[]>(HttpMethod.Get, endpointRoot + "pages").Result;
        }

        public RunePage GetRunePageById(int id)
        {
            return League.MakeApiRequestAs<RunePage>(HttpMethod.Get, endpointRoot + "page/" + id.ToString()).Result;
        }


        bool DeleteRunePage(int id)
        {
            return League.MakeApiRequest(HttpMethod.Delete, endpointRoot + "page/" + id.ToString()).Result.StatusCode == HttpStatusCode.OK;
        }

        public AddRuneResult AddRunePage(RunePage page)
        {
            var response = League.MakeApiRequestAs<Error>(HttpMethod.Post, "lol-perks/v1/pages", page).Result;

            if (response.HttpStatus != 200) // OK
            {
                if (response.Message.Equals("Max pages reached"))
                    return AddRuneResult.MaxPageReached;
            }

            return AddRuneResult.Success;
        }
    }
}
