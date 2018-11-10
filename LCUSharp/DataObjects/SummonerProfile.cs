using Newtonsoft.Json;

namespace LCUSharp.DataObjects
{
    public class SummonerProfile
    {
        [JsonProperty("accountId")]
        int AccountId { get; set; }

        [JsonProperty("displayName")]
        string DisplayName { get; set; }

        [JsonProperty("internalName")]
        string InternalName { get; set; }

        [JsonProperty("lastSeasonHighestRank")]
        string LastSeasonHighestRank { get; set; }

        [JsonProperty("percentCompleteForNextLevel")]
        int PercentCompleteForNextLevel { get; set; }

        [JsonProperty("profileIconId")]
        int ProfileIconId { get; set; }

        [JsonProperty("puuid")]
        string Puuid { get; set; }

        [JsonProperty("rerollPoints")]
        RerollPointsData RerollPoints { get; set; }

        [JsonProperty("summonerId")]
        int SummonerId { get; set; }

        [JsonProperty("summonerLevel")]
        int SummonerLevel { get; set; }

        [JsonProperty("xpSinceLastLevel")]
        int XpSinceLastLevel { get; set; }

        [JsonProperty("xpUntilNextLevel")]
        int XpUntilNextLevel { get; set; }

        public class RerollPointsData
        {
            [JsonProperty("currentPoints")]
            int CurrentPoints { get; set; }

            [JsonProperty("maxRolls")]
            int MaxRolls { get; set; }

            [JsonProperty("numberOfRolls")]
            int NumberOfRolls { get; set; }

            [JsonProperty("pointsCostToRoll")]
            int PointsCostToRoll { get; set; }

            [JsonProperty("pointsToReroll")]
            int PointsToReroll { get; set; }
        }
    }
}
