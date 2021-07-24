using VRC.Udon;
using MelonLoader;
using UnityEngine;
using VRC.Udon.Common.Interfaces;

namespace KabulClient.Features.Worlds
{
    class AmongUs
    {
        public static bool worldLoaded = false;
        public static UdonBehaviour gameLogic = null;

        public static void Initialize(string sceneName)
        {
            // TODO: Check world ID aswell.
            if (sceneName == "Skeld")
            {
                gameLogic = GameObject.Find("Game Logic")?.GetComponent<UdonBehaviour>();

                if (gameLogic != null)
                {
                    worldLoaded = true;
                }
            }
            else
            {
                worldLoaded = false;
            }
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

        /// <summary>
        /// Starts an emergency meeting, this can be spammed and the results are VERY annoying to others aswell.
        /// </summary>
        public static void EmergencyButton()
        {
            CallUdonEvent("StartMeeting");
            CallUdonEvent("SyncEmergencyMeeting");
        }

        /// <summary>
        /// Calls a UDON event.
        /// </summary>
        /// <param name="eventName">The name of the event to call.</param>
        public static void CallUdonEvent(string eventName)
        {
            gameLogic?.SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
        }

        /// Available UDON events:
        /// GetLocalPlayerNode
        /// OnLocalPlayerKillsOther  - According to soda this will play the kill sound if networked to everyone.
        /// SyncTrySabotageLights
        /// SyncDoSabotageLights
        /// SyncBodyFound
        /// SyncRepairLights
        /// SyncRepairComms
        /// SyncRepairOxygenB
    }
}
