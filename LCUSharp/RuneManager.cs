using LCUSharp.DataObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

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
            var response = League.MakeApiRequest(HttpMethod.Get, endpointRoot + "currentpage");
            return response.StaticBody<RunePage>();
        }

        public RunePage[] GetRunePages()
        {
            var response = League.MakeApiRequest(HttpMethod.Get, endpointRoot + "pages");
            return response.StaticBody<RunePage[]>();
        }

        public RunePage GetRunePageById(int id)
        {
            var response = League.MakeApiRequest(HttpMethod.Get, endpointRoot + "page/" + id.ToString());
            return response.StaticBody<RunePage>();
        }


        bool DeleteRunePage(int id)
        {
            var response = League.MakeApiRequest(HttpMethod.Delete, endpointRoot + "page/" + id.ToString());
            return response.StatusCode == HttpStatusCode.OK;
        }

        public AddRuneResult AddRunePage(RunePage page)
        {
            var response = League.MakeApiRequest(HttpMethod.Post, "lol-perks/v1/pages", page);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                var error = response.StaticBody<Error>();
                if (error.message.Equals("Max pages reached"))
                    return AddRuneResult.MaxPageReached;
            }

            return AddRuneResult.Success;
        }
    }
}
