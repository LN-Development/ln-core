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
    public List<object> GetQBPlayers()
    {
        return LNCore.Players;
    }

    public (Dictionary<string, object>, Dictionary<string, object>) GetBucketObjects()
    {
        return (LNCore.PlayerBuckets, LNCore.EntityBuckets);
    }

    public bool SetPlayerBucket(int source, string bucket)
    {
        if (source != 0 && !string.IsNullOrEmpty(bucket))
        {
            string plicense = LNCoreFunctions.GetIdentifier(source, "license");
            SetPlayerRoutingBucket(source, bucket);
            LNCore.PlayerBuckets[plicense] = new { id = source, bucket = bucket };
            return true;
        }
        else
        {
            return false;
        }
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

    public bool SetEntityBucket(int entity, string bucket)
    {
        if (entity != 0 && !string.IsNullOrEmpty(bucket))
        {
            SetEntityRoutingBucket(entity, bucket);
            LNCore.EntityBuckets[entity] = new { id = entity, bucket = bucket };
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<int> GetPlayersInBucket(string bucket)
    {
        List<int> currBucketPool = new List<int>();
        if (LNCore.PlayerBuckets != null && LNCore.PlayerBuckets.Any())
        {
            foreach (var v in LNCore.PlayerBuckets)
            {
                if (v.Value.bucket == bucket)
                {
                    currBucketPool.Add(v.Value.id);
                }
            }
            return currBucketPool;
        }
        else
        {
            return null;
        }
    }

    public List<int> GetEntitiesInBucket(string bucket)
    {
        List<int> currBucketPool = new List<int>();
        if (LNCore.EntityBuckets != null && LNCore.EntityBuckets.Any())
        {
            foreach (var v in LNCore.EntityBuckets)
            {
                if (v.Value.bucket == bucket)
                {
                    currBucketPool.Add(v.Value.id);
                }
            }
            return currBucketPool;
        }
        else
        {
            return null;
        }
    }
}