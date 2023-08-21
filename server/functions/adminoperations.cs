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
    public void Kick(int source, string reason, Action<string> setKickReason, DeferralManager deferrals)
    {
        reason = $"\n{reason}\n🔸 Check our Discord for further information: {LNCore.Config.Server.Discord}";

        if (setKickReason != null)
        {
            setKickReason(reason);
        }

        Task.Factory.StartNew(async () =>
        {
            if (deferrals != null)
            {
                deferrals.Update(reason);
                await BaseScript.Delay(2500);
            }

            if (source != -1)
            {
                API.DropPlayer(source, reason);
            }

            for (int i = 0; i < 4; i++)
            {
                while (true)
                {
                    if (source != -1)
                    {
                        if (API.GetPlayerPing(source) >= 0)
                        {
                            break;
                        }
                        await BaseScript.Delay(100);
                        await Task.Factory.StartNew(() => API.DropPlayer(source, reason));
                    }
                }
                await BaseScript.Delay(5000);
            }
        });
    }

    public void AddPermission(int source, string permission)
    {
        if (!API.IsPlayerAceAllowed(source, permission))
        {
            API.ExecuteCommand($"add_principal player.{source} lnCore.{permission}");
            API.TriggerEvent("chatMessage", "[Server]", new IntPtr(-1), $"Permission '{permission}' added for player {source}");
        }
    }

    public void RemovePermission(int source, string permission = null)
    {
        if (permission != null)
        {
            if (API.IsPlayerAceAllowed(source, permission))
            {
                API.ExecuteCommand($"remove_principal player.{source} lnCore.{permission}");
                API.TriggerEvent("chatMessage", "[Server]", new IntPtr(-1), $"Permission '{permission}' removed for player {source}");
            }
        }
        else
        {
            dynamic permissions = LNCore.Config.Server.Permissions;

            foreach (string v in permissions)
            {
                if (API.IsPlayerAceAllowed(source, v))
                {
                    API.ExecuteCommand($"remove_principal player.{source} lnCore.{v}");
                    API.TriggerEvent("chatMessage", "[Server]", new IntPtr(-1), $"Permission '{v}' removed for player {source}");
                }
            }
        }
    }

    public bool HasPermission(int source, dynamic permission)
    {
        if (permission is string)
        {
            if (API.IsPlayerAceAllowed(source, permission))
            {
                return true;
            }
        }
        else if (permission is object[])
        {
            foreach (string permLevel in permission)
            {
                if (API.IsPlayerAceAllowed(source, permLevel))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public Dictionary<string, bool> GetPermission(int source)
    {
        Dictionary<string, bool> perms = new Dictionary<string, bool>();
        dynamic permissions = LNCore.Config.Server.Permissions;

        foreach (string v in permissions)
        {
            if (API.IsPlayerAceAllowed(source, v))
            {
                perms[v] = true;
            }
        }

        return perms;
    }

    public bool IsOptin(int source)
    {
        string license = LNCore.Functions.GetIdentifier(source, "license");

        if (string.IsNullOrEmpty(license) || !HasPermission(source, "admin"))
        {
            return false;
        }

        dynamic player = LNCore.Functions.GetPlayer(source);
        return player.PlayerData.optin;
    }

    public bool HasPermission(int source, string permission)
    {
        return API.IsPlayerAceAllowed(source, $"lnCore.{permission}");
    }
    public void ToggleOptin(int source)
    {
        string license = LNCore.Functions.GetIdentifier(source, "license");

        if (string.IsNullOrEmpty(license) || !HasPermission(source, "admin"))
        {
            return;
        }

        dynamic player = LNCore.Functions.GetPlayer(source);
        player.PlayerData.optin = !player.PlayerData.optin;
        player.Functions.SetPlayerData("optin", player.PlayerData.optin);
    }

    public bool HasPermission(int source, string permission)
    {
        return API.IsPlayerAceAllowed(source, $"lnCore.{permission}");
    }
}