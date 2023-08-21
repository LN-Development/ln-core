using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;


public class LNCoreFunctions
{
    public Vehicle SpawnVehicle(int source, dynamic model, Vector4 coords = null, bool warp = false)
    {
        Ped ped = new Ped(API.GetPlayerPed(source));
        uint modelHash = (model is string) ? API.GetHashKey(model) : (uint)model;

        if (coords == null)
        {
            coords = ped.Position;
        }

        float heading = (coords.W != null) ? (float)coords.W : 0.0f;
        Vehicle veh = API.CreateVehicle(modelHash, coords.X, coords.Y, coords.Z, heading, true, true);

        while (!API.DoesEntityExist(veh))
        {
            API.Wait(0);
        }

        if (warp)
        {
            while (API.GetVehiclePedIsIn(ped) != veh)
            {
                API.Wait(0);
                API.TaskWarpPedIntoVehicle(ped.Handle, veh.Handle, -1);
            }
        }

        while (API.NetworkGetEntityOwner(veh.Handle) != source)
        {
            API.Wait(0);
        }

        return new Vehicle(veh.Handle);
    }

    public Vehicle CreateAutomobile(int source, dynamic model, Vector4 coords = null, bool warp = false)
    {
        uint modelHash = (model is string) ? API.GetHashKey(model) : (uint)model;

        if (coords == null)
        {
            coords = API.GetEntityCoords(API.GetPlayerPed(source));
        }

        float heading = (coords.W != null) ? (float)coords.W : 0.0f;
        uint createAutomobileHash = API.GetHashKey("CREATE_AUTOMOBILE");
        IntPtr createAutomobilePtr = API.GetProcAddress(createAutomobileHash);

        if (createAutomobilePtr != IntPtr.Zero)
        {
            var createAutomobile = (Func<uint, Vector4, float, bool, bool, int>)Marshal.GetDelegateForFunctionPointer(
                createAutomobilePtr, typeof(Func<uint, Vector4, float, bool, bool, int>));

            int veh = createAutomobile(modelHash, coords, heading, true, true);

            while (!API.DoesEntityExist(veh))
            {
                API.Wait(0);
            }

            if (warp)
            {
                API.TaskWarpPedIntoVehicle(API.GetPlayerPed(source), veh, -1);
            }

            return new Vehicle(veh);
        }
        else
        {
            return null;
        }
    }

    public Vehicle CreateVehicle(int source, dynamic model, dynamic vehtype, Vector4 coords = null, bool warp = false)
    {
        uint modelHash = (model is string) ? API.GetHashKey(model) : (uint)model;
        string vehicleType = (vehtype is string) ? vehtype.ToString() : vehtype.ToString();

        if (coords == null)
        {
            coords = API.GetEntityCoords(API.GetPlayerPed(source));
        }

        float heading = (coords.W != null) ? (float)coords.W : 0.0f;
        uint createVehicleServerSetterHash = API.GetHashKey("CREATE_VEHICLE_SERVER_SETTER");
        IntPtr createVehicleServerSetterPtr = API.GetProcAddress(createVehicleServerSetterHash);

        if (createVehicleServerSetterPtr != IntPtr.Zero)
        {
            var createVehicleServerSetter = (Func<uint, string, Vector4, float, int>)Marshal.GetDelegateForFunctionPointer(
                createVehicleServerSetterPtr, typeof(Func<uint, string, Vector4, float, int>));

            int veh = createVehicleServerSetter(modelHash, vehicleType, coords, heading);

            while (!API.DoesEntityExist(veh))
            {
                API.Wait(0);
            }

            if (warp)
            {
                API.TaskWarpPedIntoVehicle(API.GetPlayerPed(source), veh, -1);
            }

            return new Vehicle(veh);
        }
        else
        {
            return null;
        }
    }
}