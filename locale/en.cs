using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


public class Translations
{
    public Dictionary<string, string> Success { get; set; } = new Dictionary<string, string>
    {
        { "server_opened", "The server has been opened" },
        { "server_closed", "The server has been closed" },
    };
}