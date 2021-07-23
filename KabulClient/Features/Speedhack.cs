using System;
using MelonLoader;

namespace KabulClient.Features
{
    class Speedhack
    {
        public static bool speedEnabled = false;
        public static float speedMultiplier = 5;

        private static float originalWalkSpeed = 0f;
        private static float originalRunSpeed = 0f;
        private static float originalStrafeSpeed = 0f;

        public static void Toggle()
        {
            // MelonLogger.Msg("Speedhack toggled.");
            speedEnabled = !speedEnabled;
        }

        public static void Main()
        {
            try
            {
                VRCPlayer localPlayer = Utils.GetLocalPlayer();

                // Make sure our player is valid.
                if (localPlayer == null || localPlayer?.prop_VRCPlayerApi_0 == null)
                {
                    return;
                }

                if (originalWalkSpeed == 0f || originalRunSpeed == 0f || originalStrafeSpeed == 0f)
                {
                    originalWalkSpeed = localPlayer.prop_VRCPlayerApi_0.GetWalkSpeed();
                    originalRunSpeed = localPlayer.prop_VRCPlayerApi_0.GetRunSpeed();
                    originalStrafeSpeed = localPlayer.prop_VRCPlayerApi_0.GetStrafeSpeed();
                }

                if (speedEnabled)
                {
                    localPlayer.prop_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed * speedMultiplier);
                    localPlayer.prop_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed * speedMultiplier);
                    localPlayer.prop_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed * speedMultiplier);
                }
                else
                {
                    localPlayer.prop_VRCPlayerApi_0.SetWalkSpeed(originalWalkSpeed);
                    localPlayer.prop_VRCPlayerApi_0.SetRunSpeed(originalRunSpeed);
                    localPlayer.prop_VRCPlayerApi_0.SetStrafeSpeed(originalStrafeSpeed);
                    originalRunSpeed = 0f;
                    originalWalkSpeed = 0f;
                    originalStrafeSpeed = 0f;
                }
            } 
            catch (Exception e)
            {
                // SHUT THE FUCK UP NULL REFERENCE EXCEPTION.
                MelonLogger.Msg("Swallowing caught exception in Speedhack.Main().");
            }
        }
    }
}
