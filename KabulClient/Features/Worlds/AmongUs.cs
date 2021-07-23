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
            return sceneName == "Skeld";
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
            GameObject emergencyButton = GameObject.Find("Game Logic/Emergency meeting button");

            if (emergencyButton == null)
            {
                return;
            }

            // TODO: Find out how to activate the button.
        }
    }
}
