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
    public void CreateUseableItem(string item, dynamic data)
    {
        LNCore.UsableItems[item] = data;
    }


    public dynamic CanUseItem(string item)
    {
        if (LNCore.UsableItems.ContainsKey(item))
        {
            return LNCore.UsableItems[item];
        }
        else
        {
            return null;
        }
    }

    public void UseItem(int source, string item)
    {
        if (API.GetResourceState("ln-inventory") == "missing") return;
        API.Export("ln-inventory", "UseItem", source, item);
    }

    public bool HasItem(int source, string items, int amount)
    {
        if (API.GetResourceState("qb-inventory") == "missing")
        {
            return false;
        }

        return API.Export<bool>("qb-inventory", "HasItem", source, items, amount);
    }
}