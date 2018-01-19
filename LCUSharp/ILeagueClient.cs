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
        RunePage GetCurrentRunePage();
        RunePage[] GetRunePages();
        AddRuneResult AddRunePage(RunePage rune);
    }
}
