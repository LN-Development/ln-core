using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

public class LNCore
{
    public Dictionary<string, object> Functions { get; set; } = new Dictionary<string, object>();
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
    //Pega informação do player
    public static object GetPlayerData(Action<object> cb = null)
    {
        if (cb == null)
            return LNCore.PlayerData;

        cb(LNCore.PlayerData);
        return null;
    }
    //Pega coordenada do player
    public static Vector4 GetCoords(int entity)
    {
        Vector3 coords = API.GetEntityCoords(entity);
        return new Vector4(coords.X, coords.Y, coords.Z, API.GetEntityHeading(entity));
    }
}


