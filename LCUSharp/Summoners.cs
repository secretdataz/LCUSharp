using LCUSharp.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace LCUSharp
{
    public class Summoners
    {
        ILeagueClient League;
        const string endpointRoot = "/lol-summoner/v1/";

        public Summoners(ILeagueClient league)
        {
            League = league;
        }

        public bool IsNameAvailable(string name)
        {
            var result = League.MakeApiRequest(HttpMethod.Get, endpointRoot + "check-name-availability/" + name).Result;
            return bool.Parse(result.Content.ReadAsStringAsync().Result);
        }

        public SummonerProfile GetCurrentSummoner()
        {
            return League.MakeApiRequestAs<SummonerProfile>(HttpMethod.Get, endpointRoot + "current-summoner").Result;
        }

        public SummonerProfile GetSummonerProfile(string name)
        {
            return League.MakeApiRequestAs<SummonerProfile>(HttpMethod.Get, endpointRoot + "summoners/" + name).Result;
        }
    }
}
