using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


public class LNCorePlayerData
{
    // Defina aqui os campos de dados dos jogadores, se necessário
}

public class LNCore
{
    public Dictionary<string, object> PlayerData { get; set; } = new Dictionary<string, object>();
    public LNConfig Config { get; set; }
    public LNShared Shared { get; set; }
    public Dictionary<string, object> ClientCallbacks { get; set; } = new Dictionary<string, object>();
    public Dictionary<string, object> ServerCallbacks { get; set; } = new Dictionary<string, object>();
}

public static class Exports
{
    public static LNCore GetCoreObject()
    {
        QBCore lnCore = new LNCore();
        return lnCore;
    }
}