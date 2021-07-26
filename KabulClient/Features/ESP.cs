using VRC;
using VRC.Core;
using UnityEngine;
using MelonLoader;
using System;

namespace KabulClient.Features
{
    class ESP
    {
        public static bool espEnabled = false;
        public static bool linesEnabled = false;
        public static float espRainbowSpeed = 0.1f;
        
        /// <summary>
        /// Responsible for toggling ESP on or off.
        /// </summary>
        public static void Toggle()
        {
            MelonLogger.Msg("ESP toggled.");

            if (!espEnabled)
            {
                espEnabled = !espEnabled;
            }
            else
            {
                espEnabled = !espEnabled;
                
                // This code is required to disable SelectRegionESP.

                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

                foreach (GameObject playerObject in playerObjects)
                {
                    if (playerObject.transform.Find("SelectRegion"))
                    {
                        playerObject.transform.Find("SelectRegion").GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                        playerObject.transform.Find("SelectRegion").GetComponent<Renderer>().sharedMaterial.SetColor("_Color", Color.red);
                        Utils.ToggleOutline(playerObject.transform.Find("SelectRegion").GetComponent<Renderer>(), false);
                    }
                }
            }
        }

        public static void LineESP()
        {
            if (espEnabled && linesEnabled)
            {
                GameObject localPlayer = Utils.GetLocalPlayer()?.gameObject;
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

                // Loop through all the player objects in the world.
                foreach (GameObject playerObject in playerObjects)
                {
                    // Make sure the player is valid and isn't our local player.
                    if (playerObject == null || playerObject == localPlayer)
                    {
                        continue;
                    }

                    // Render ESP for users.
                    Drawing.DrawESPLine(playerObject, Utils.HSBColor.ToColor(new Utils.HSBColor(Mathf.PingPong(Time.time * espRainbowSpeed, 1), 1, 1)));
                }
            }
        }

        public static void UserInformationESP()
        {
            if (espEnabled)
            {
                Camera localCamera = GameObject.Find("Camera (eye)")?.GetComponent<Camera>();

                if (localCamera == null)
                {
                    MelonLogger.Msg("localCamera is null!");
                    return;
                }

                // This might be a bit expensive.
                foreach (Player player in Utils.GetAllPlayers())
                {
                    try
                    {
                        if (player == null)
                        {
                            continue;
                        }

                        APIUser apiUser = player.field_Private_APIUser_0;

                        if (apiUser == null)
                        {
                            continue;
                        }

                        // TODO: Make the offset height from the player for the text dependant on the player's avatar height.
                        Vector3 worldToScreenPos = localCamera.WorldToScreenPoint(player.transform.position + new Vector3(0, 1, 0));
                        worldToScreenPos.y = Screen.height - worldToScreenPos.y;

                        // Make sure the player isn't behind us, otherwise don't render the text.
                        if (worldToScreenPos.z <= 0)
                        {
                            continue;
                        }

                        float yOffset = worldToScreenPos.y;

                        // Render text.

                        // NOTE: The rainbow color can sometimes be hard to see with some colors.
                        GUI.contentColor = Utils.HSBColor.ToColor(new Utils.HSBColor(Mathf.PingPong(Time.time * espRainbowSpeed, 1), 1, 1));
                        GUI.Label(new Rect(worldToScreenPos.x + 20, yOffset, 1000, 100), apiUser.displayName); yOffset += 20;
                        GUI.contentColor = Color.white;

                        // NOTE: This only shows friends added after this code was added.
                        // TODO: Fix the bug mentioned above.
                        if (apiUser.isFriend)
                        {
                            // This is our friend :)
                            GUI.contentColor = Color.yellow;
                            GUI.Label(new Rect(worldToScreenPos.x + 20, yOffset, 1000, 100), "FRIEND"); yOffset += 20;
                            GUI.contentColor = Color.white;
                        }

                        if (apiUser.hasTrustedTrustLevel || apiUser.hasVeteranTrustLevel)
                        {
                            // This person is mentally ill and should be flagged as such.
                            GUI.contentColor = Color.magenta;
                            GUI.Label(new Rect(worldToScreenPos.x + 20, yOffset, 1000, 100), "MENTALLY ILL"); yOffset += 20;
                            GUI.contentColor = Color.white;
                        }

                        // GUI.Label(new Rect(worldToScreenPos.x + 20, yOffset, 1000, 100), $"ID: {apiUser.id}"); yOffset += 20;

                        GUI.contentColor = Color.white;
                    }
                    catch (Exception e)
                    {
                        MelonLogger.Msg("Swallowing caught exception in ESP.UserInformationESP().");
                    }
                }
            }
        }

        private static void SelectRegionESP(GameObject player)
        {
            if (player.transform.Find("SelectRegion"))
            {
                // Get the selection region for the player.
                var renderer = player.transform.Find("SelectRegion").GetComponent<Renderer>();

                if (renderer == null)
                {
                    return;
                }

                // Toggle the player outline.
                Utils.ToggleOutline(renderer, true);
            }
        }

        public static void UpdateColors()
        {
            if (HighlightsFX.prop_HighlightsFX_0 == null || HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0 == null)
            {
                return;
            }

            HighlightsFX.prop_HighlightsFX_0.field_Protected_Material_0.SetColor(
                "_HighlightColor", espEnabled ? Utils.HSBColor.ToColor(
                    new Utils.HSBColor(Mathf.PingPong(Time.time * espRainbowSpeed, 1), 1, 1)) : new Color(0f, 0.573f, 1f, 1f));
        }

        public static void Main()
        {
            if (espEnabled)
            {
                GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

                // Loop through all the player objects in the world.
                for (int i = 0; i < playerObjects.Length; i++)
                {
                    GameObject playerObject = playerObjects[i];
                    
                    // Make sure the player is valid.
                    if (playerObject == null)
                    {
                        continue;
                    }

                    // Render ESP for users.
                    SelectRegionESP(playerObject);
                }
            }
        }
    }
}
