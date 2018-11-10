using LCUSharp.DataObjects;
using Newtonsoft.Json;
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

        public int GetOwnedPageCount()
        {
            var result = League.MakeApiRequest(HttpMethod.Get, endpointRoot + "inventory").Result;
            return int.Parse(result.Content.ReadAsStringAsync().Result);
        }

        bool DeleteRunePage(int id)
        {
            return League.MakeApiRequest(HttpMethod.Delete, endpointRoot + "page/" + id.ToString()).Result.StatusCode == HttpStatusCode.OK;
        }

        public AddRuneResult AddRunePage(RunePage page)
        {
            var response = League.MakeApiRequest(HttpMethod.Post, endpointRoot + "pages", page).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var error = JsonConvert.DeserializeObject<Error>(json);
                if (error.Message.Equals("Max pages reached"))
                    return AddRuneResult.MaxPageReached;
            }

            return AddRuneResult.Success;
        }
    }
}
