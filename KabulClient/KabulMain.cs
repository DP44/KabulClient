using System.Linq;
using UnityEngine;
using MelonLoader;
using UIExpansionKit.API;
using VRC;
using VRC.Core;

[assembly: MelonInfo(typeof(KabulClient.KabulMain), "Kabul Client", "0.0.5", "DonkeyPounder44")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace KabulClient
{
    public class KabulMain : MelonMod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("OnApplicationStart().");

            MelonLogger.Msg("Hooking NetworkManager.");
            Hooks.NetworkManagerHook.Initialize();
            Hooks.NetworkManagerHook.OnJoin += OnPlayerJoined;
            Hooks.NetworkManagerHook.OnLeave += OnPlayerLeft;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Features.ESP.UpdatePlayerObjectList();
            MelonLogger.Msg($"OnSceneWasLoaded({buildIndex}, \"{sceneName}\")");
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            MelonLogger.Msg($"OnSceneWasInitialized({buildIndex}, \"{sceneName}\")");

            Features.Worlds.JustBClub.Initialize(sceneName);
            Features.Worlds.AmongUs.Initialize(sceneName);

            base.OnSceneWasInitialized(buildIndex, sceneName);
        }

        public override void OnUpdate()
        {
            // Toggling our menu.
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                Menu.ToggleMenu();
            }

            // Speedhack.
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyUp(KeyCode.X))
            {
                Features.Speedhack.Toggle();
            }

            Features.ESP.UpdateColors();
            Features.ESP.Main();
            Features.Speedhack.Main();
        }

        public override void OnGUI()
        {
            // Handle menu rendering.
            Menu.Main();

            // Draw text for ESP.
            Features.ESP.UserInformationESP();

            // Handle cursor locking to allow interaction with our menu.
            Menu.HandleCursor();

            base.OnGUI();
        }

        public void OnPlayerJoined(Player player)
		{
            APIUser apiUser = player.prop_APIUser_0;

            if (apiUser == null)
            {
                return;
            }

            MelonLogger.Msg($"Player \"{apiUser.displayName}\" joined.");
            Features.ESP.UpdatePlayerObjectList();
        }

        public void OnPlayerLeft(Player player)
        {
            APIUser apiUser = player.prop_APIUser_0;

            if (apiUser == null)
            {
                return;
            }

            MelonLogger.Msg($"Player \"{apiUser.displayName}\" left.");
            Features.ESP.UpdatePlayerObjectList();
        }
    }
}
