using VRC.Udon;
using MelonLoader;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace KabulClient.Features.Worlds
{
    class AmongUs
    {
        public static bool worldLoaded = false;

        public static void Initialize(string sceneName)
        {
            worldLoaded = sceneName == "Skeld";
        }

        public static void ToggleSabotageHud(bool value)
        {
            GameObject sabotageHud = GameObject.Find("Game Logic/Sabotage HUD");

            if (sabotageHud == null)
            {
                return;
            }

            sabotageHud.SetActive(value);
        }

        public static void EmergencyButton()
        {
            UdonBehaviour gameLogic = GameObject.Find("Game Logic")?.GetComponent<UdonBehaviour>();

            if (gameLogic == null)
            {
                return;
            }

            MelonLogger.Msg("StartMeeting called.");
            gameLogic.SendCustomNetworkEvent(NetworkEventTarget.All, "StartMeeting");
            gameLogic.SendCustomNetworkEvent(NetworkEventTarget.All, "SyncEmergencyMeeting");
        }
    }
}
