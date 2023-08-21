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
    public void Notify(int source, string text, string type, int length)
    {
        API.TriggerClientEvent("LNCore:Notify", source, text, type, length);
    }
}