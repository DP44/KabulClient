using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MelonLoader;
using VRCSDK2;

namespace KabulClient.Features
{
    class Noclip
    {
        private static float noclipSpeed = 4;
        private static bool noclipEnabled = false;
        public static List<int> noclipToEnable = new List<int>();

        public static bool NoclipEnabled
        {
            get
            {
                return noclipEnabled;
            }
            set
            {
                noclipEnabled = value;
            }
        }

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

            if (Input.GetAxis("Vertical") != 0f)
            {
                localPlayer.transform.position += cameraTransform.transform.forward * Time.deltaTime * Input.GetAxis("Vertical") * noclipSpeed * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
            }

            if (Input.GetAxis("Horizontal") != 0f)
            {
                localPlayer.transform.position += cameraTransform.transform.right * Time.deltaTime * Input.GetAxis("Horizontal") * noclipSpeed * (Input.GetKey(KeyCode.LeftShift) ? 4f : 2f);
            }
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
