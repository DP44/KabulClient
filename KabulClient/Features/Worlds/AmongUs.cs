using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Udon;

namespace KabulClient.Features.Worlds
{
    class AmongUs
    {
        public static bool worldLoaded = false;

        public static void Initialize(string sceneName)
        {
            if (sceneName == "Skeld")
            {
                worldLoaded = true;
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

        public static void EmergencyButton()
        {
            // NOTE: The UdonBehaviour interactable string is "EMERGENCY".
            GameObject emergencyButton = GameObject.Find("Game Logic/Emergency meeting button");

            if (emergencyButton == null)
            {
                return;
            }

            UdonBehaviour buttonBehaviour = emergencyButton?.GetComponent<UdonBehaviour>();


        }
    }
}
