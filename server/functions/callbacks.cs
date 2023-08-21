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
    public void TriggerClientCallback(string name, int source, Delegate cb, params object[] args)
    {
        LNCore.ClientCallbacks[name] = cb;
        TriggerClientEvent("LNCore:Client:TriggerClientCallback", source, name, args);
    }

    //Exemplo de Trigger
/*
    private void TriggerClientEvent(string eventName, params object[] args)
    {



        API.TriggerClientEvent(eventName, args);
    }
*/
    public void CreateCallback(string name, Delegate cb)
    {
        LNCore.ServerCallbacks[name] = cb;
    }

    public void TriggerCallback(string name, int source, Delegate cb, params object[] args)
    {
        if (!LNCore.ServerCallbacks.ContainsKey(name)) return;
        
        LNCore.ServerCallbacks[name].DynamicInvoke(source, cb, args);
    }
}