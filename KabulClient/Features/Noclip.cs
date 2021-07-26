using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MelonLoader;
using VRCSDK2;

namespace KabulClient.Features
{
    class Noclip
    {
        public static bool noclipEnabled = false;
        public static List<int> noclipToEnable = new List<int>();

        /// <summary>
        /// Used to toggle noclip.
        /// </summary>
        public static void Toggle()
        {
            MelonLogger.Msg("Noclip toggled.");
            noclipEnabled = !noclipEnabled;
            DoNoclip();
        }

        /// <summary>
        /// The main noclip code.
        /// </summary>
        public static void Main()
        {
            // TODO: Fix weird gravity bug.
            VRCPlayer localPlayer = Utils.GetLocalPlayer();
            Transform cameraTransform = Camera.main.transform;

            // This is gross to look at.

            // Vertical movement.
            if (Input.GetKey(KeyCode.Q)) localPlayer.gameObject.transform.position = localPlayer.transform.position - new Vector3(0f, Speedhack.speedMultiplier * Time.deltaTime, 0f);
            if (Input.GetKey(KeyCode.E)) localPlayer.gameObject.transform.position = localPlayer.transform.position + new Vector3(0f, Speedhack.speedMultiplier * Time.deltaTime, 0f);

            // Directional movement.
            if (Input.GetKey(KeyCode.W)) localPlayer.transform.position += localPlayer.transform.forward * Speedhack.speedMultiplier * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) localPlayer.transform.position += localPlayer.transform.right * -1f * Speedhack.speedMultiplier * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) localPlayer.transform.position += localPlayer.transform.forward * -1f * Speedhack.speedMultiplier * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) localPlayer.transform.position += localPlayer.transform.right * Speedhack.speedMultiplier * Time.deltaTime;
        }

        /// <summary>
        /// Disables collision on all objects.
        /// </summary>
        public static void DoNoclip()
        {
            Physics.gravity = noclipEnabled ? new Vector3(0, 0, 0) : new Vector3(0, -9.81f, 0);

            Collider[] colliders = GameObject.FindObjectsOfType<Collider>();
            Component playerCollider = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponents<Collider>().FirstOrDefault<Component>();

            foreach (Collider collider in colliders)
            {
                bool isImportant = collider.GetComponent<PlayerSelector>() != null || collider.GetComponent<VRC_Pickup>() != null || collider.GetComponent<QuickMenu>() != null || collider.GetComponent<VRC_Station>() != null || collider.GetComponent<VRC_AvatarPedestal>() != null;

                if (isImportant)
                {
                    collider.enabled = true;
                }
                else
                {
                    // Check if the collider isn't already disabled or if we have that item stored already.
                    bool isValid = collider != playerCollider && ((noclipEnabled && collider.enabled || (!noclipEnabled && noclipToEnable.Contains(collider.GetInstanceID()))));

                    if (isValid)
                    {
                        collider.enabled = !noclipEnabled;

                        if (noclipEnabled)
                        {
                            noclipToEnable.Add(collider.GetInstanceID());
                        }
                    }
                }
            }

            if (!noclipEnabled)
            {
                noclipToEnable.Clear();
            }
        }
    }
}
