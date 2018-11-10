using Newtonsoft.Json;

namespace LCUSharp.DataObjects
{
    public class RunePage
    {
        [JsonProperty("current")]
        public bool IsCurrentPage { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("formatVersion")]
        public int FormatVersion;
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("isDeletable")]
        public bool IsDeletable { get; set; }
        [JsonProperty("isEditable")]
        public bool IsEditable { get; set; }
        [JsonProperty("isValid")]
        public bool IsValid { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("primaryStyleId")]
        public int PrimaryTreeId { get; set; }
        [JsonProperty("selectedPerkIds")]
        public int[] SelectedRunes { get; set; }
        [JsonProperty("subStyleId")]
        public int SecondaryTreeId { get; set; }

        public RunePage()
        {
            FormatVersion = 4;
            IsDeletable = true;
            IsEditable = true;
            IsValid = true;
        }
    }

    public enum Rune
    {
        // Precision tree
        Precision = 8000,
        PressTheAttack = 8005,
        LethalTempo = 8008,
        Conqueror = 8010,
        FleetFootwork = 8021,
        Overheal = 9101,
        Triumph = 9111,
        PresenceOfMind = 8009,
        LegendBloodline = 9103,
        LegendAlacrity,
        LegendTenacity,
        CoupDeGrace = 8014,
        CutDown = 8017,
        LastStand = 8299,

        // Domination tree
        Domination = 8100,
        UltimateHunter = 8106,
        Electrocute = 8112,
        Predator = 8124,
        DarkHarvest = 8128,
        CheapShot = 8126,
        TasteOfBlood = 8139,
        SuddenImpact = 8143,
        ZombieWard = 8136,
        GhostPoro = 8120,
        EyeballCollection = 8138,
        RavenousHunter = 8135,
        IngeniousHunter = 8134,
        RelentlessHunter = 8105,
        HailOfBlades = 9923,

        // Sorcery tree
        Sorcery = 8200,
        SummonAery = 8214,
        ArcaneComet = 8229,
        PhaseRush = 8230,
        NullifyingOrb = 8224,
        ManaflowBand = 8226,
        TheUltimateHat = 8243,
        Transcendence = 8210,
        Celerity = 8234,
        AbsoluteFocus = 8233,
        Scorch = 8237,
        Waterwalking = 8232,
        GatheringStorm = 8236,
        NimbusCloak = 8275,

        // Resolve tree
        Resolve = 8400,
        GraspOfTheUndying = 8437,
        Aftershock = 8439,
        Guardian = 8465,
        Unflinching = 8242,
        Demolish = 8446,
        FontOfLife = 8463,
        IronSkin = 8430,
        MirrorShell = 8435,
        Conditioning = 8429,
        Overgrowth = 8451,
        Revitalize = 8453,
        SecondWind = 8444,
        Chrysalis = 8472,
        BonePlating = 8473,

        // Inspiration tree
        Inspiration = 8300,
        UnsealedSpellbook = 8326,
        GlacialAugment = 8351,
        Kleptomancy = 8359,
        HextechFlashtraption = 8306,
        BiscuitDelivery = 8345,
        PerfectTiming = 8313,
        MagicalFootwear = 8304,
        FuturesMarket = 8321,
        MinionDematerializer = 8316,
        CosmicInsight = 8347,
        ApproachVelocity = 8410,
        CelestialBody = 8339,
        TimeWarpTonic = 8352,
    }

    public enum AddRuneResult
    {
        // WIP
        Success,
        MaxPageReached
    }
}
