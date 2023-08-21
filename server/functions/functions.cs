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

    public static Vector4 GetCoords(int entity)
    {
        Vector3 coords = API.GetEntityCoords(entity, false);
        float heading = API.GetEntityHeading(entity);
        return new Vector4(coords.X, coords.Y, coords.Z, heading);
    }
    
    public static string GetIdentifier(int source, string idtype)
    {
        var identifiers = API.GetPlayerIdentifiers(source).ToList();
        foreach (var identifier in identifiers)
        {
            if (identifier.Contains(idtype))
            {
                return identifier;
            }
        }
        return null;
    }

    public bool IsLicenseInUse(string license)
    {
        var players = API.GetPlayers();
        
        foreach (int player in players)
        {
            var identifiers = API.GetPlayerIdentifiers(player);
            
            if (identifiers.Any(id => id.Contains("license") && id == license))
            {
                return true;
            }
        }
        
        return false;
    }


}


