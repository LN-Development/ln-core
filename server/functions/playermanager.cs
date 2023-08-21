using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

public class LNCore
{
    public LNConfig Config { get; set; }
    public LNShared Shared { get; set; }
    public Dictionary<string, object> ClientCallbacks { get; set; } = new Dictionary<string, object>();
    public Dictionary<string, object> ServerCallbacks { get; set; } = new Dictionary<string, object>();
}

public static class Exports
{
    public static LNCore GetCoreObject()
    {
        LNCore lnCore = new LNCore();
        return lnCore;
    }
}


public class LNCoreFunctions
{
    public List<int> GetPlayers()
    {
        List<int> sources = new List<int>();

        foreach (int k in LNCore.Players.Keys)
        {
            sources.Add(k);
        }

        return sources;
    }


    public Dictionary<int, dynamic> GetLNPlayers()
    {
        return LNCore.Players;
    }



    public Tuple<List<int>, int> GetPlayersOnDuty(string job)
    {
        List<int> players = new List<int>();
        int count = 0;

        foreach (KeyValuePair<int, dynamic> playerEntry in LNCore.Players)
        {
            dynamic player = playerEntry.Value;

            if (player.PlayerData.job.name == job && player.PlayerData.job.onduty)
            {
                players.Add(playerEntry.Key);
                count++;
            }
        }

        return new Tuple<List<int>, int>(players, count);
    }

    public int GetDutyCount(string job)
    {
        int count = 0;

        foreach (KeyValuePair<int, dynamic> playerEntry in LNCore.Players)
        {
            dynamic player = playerEntry.Value;

            if (player.PlayerData.job.name == job && player.PlayerData.job.onduty)
            {
                count++;
            }
        }

        return count;
    }
}