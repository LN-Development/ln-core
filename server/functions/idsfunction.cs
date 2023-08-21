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

    public Dictionary<string, object> Functions { get; set; } = new Dictionary<string, object>();
    public Dictionary<string, object> PlayerBuckets { get; set; } = new Dictionary<string, object>();
    public Dictionary<string, object> EntityBuckets { get; set; } = new Dictionary<string, object>();
    public Dictionary<string, object> UsableItems { get; set; } = new Dictionary<string, object>();
    public dynamic GetPlayer(int source)
    {
        if (source is int)
        {
            return LNCore.Players[source];
        }
        else
        {
            return LNCore.Players[GetSource(source)];
        }
    }

    public dynamic GetPlayerByCitizenId(string citizenid)
    {
        foreach (int src in LNCore.Players.Keys)
        {
            if (LNCore.Players[src].PlayerData.citizenid == citizenid)
            {
                return LNCore.Players[src];
            }
        }
        return null;
    }
    public dynamic GetOfflinePlayerByCitizenId(string citizenid)
    {
        return LNCore.Player.GetOfflinePlayer(citizenid);
    }


    public dynamic GetPlayerByPhone(string number)
    {
        foreach (int src in LNCore.Players.Keys)
        {
            if (LNCore.Players[src].PlayerData.charinfo.phone == number)
            {
                return LNCore.Players[src];
            }
        }
        return null;
    }
}