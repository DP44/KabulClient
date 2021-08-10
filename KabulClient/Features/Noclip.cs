using System.Collections.Generic;
using KabulClient.Hooks;
using VRC.Animation;
using System.Linq;
using UnityEngine;
using MelonLoader;
using VRCSDK2;
using VRC;

namespace KabulClient.Features
{
    class Noclip
    {
        public static bool noclipEnabled = false;
        public static List<int> noclipToEnable = new List<int>();

        private static VRCMotionState motionState;
        private static InputStateController stateController;

        // We use this to store the original value to change back to.
        private static Vector3 originalGravity;

        /// <summary>
        /// Used to toggle noclip.
        /// </summary>
        public static void Toggle()
        {
            MelonLogger.Msg("Noclip toggled.");
           
            if (!noclipEnabled)
            {
                noclipEnabled = !noclipEnabled;
            }
            else
            {
                noclipEnabled = !noclipEnabled;

                VRCPlayer localPlayer = Utils.GetLocalPlayer();
                localPlayer.GetComponent<VRCMotionState>().field_Private_CharacterController_0.enabled = true;
            }
        }

        /// <summary>
        /// The main noclip code.
        /// </summary>
        public static void Main()
        {
            VRCPlayer localPlayer = Utils.GetLocalPlayer();

            if (localPlayer != null && localPlayer.gameObject != null && RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                // Keep track of our motion state.
                if (motionState == null)
                {
                    motionState = localPlayer.GetComponent<VRCMotionState>();
                }

                // Store our input state controller.
                if (stateController == null)
                {
                    stateController = localPlayer.GetComponent<InputStateController>();
                }

                Physics.gravity = noclipEnabled ? new Vector3(0, 0, 0) : new Vector3(0, -9.81f, 0);

                if (noclipEnabled)
                {
                    // TODO: Fix weird gravity bug.
                    Transform cameraTransform = Camera.main.transform;

                    if (Input.GetAxis("Vertical") != 0f)
                    {
                        localPlayer.transform.position += cameraTransform.forward * Time.deltaTime * Input.GetAxis("Vertical") * 
                            ((Speedhack.speedEnabled || Input.GetKey(KeyCode.LeftShift)) ? Speedhack.speedMultiplier : 2);
                    }

                    if (Input.GetAxis("Horizontal") != 0f)
                    {
                        localPlayer.transform.position += cameraTransform.right * Time.deltaTime * Input.GetAxis("Horizontal") *
                            ((Speedhack.speedEnabled || Input.GetKey(KeyCode.LeftShift)) ? Speedhack.speedMultiplier : 2);
                    }

                    if (motionState != null)
                    {
                        motionState.Reset();

                        if (motionState.field_Private_CharacterController_0 != null)
                        {
                            motionState.field_Private_CharacterController_0.enabled = false;
                        }
                    }
                }

                if (stateController != null && !noclipEnabled)
                {
                    stateController.ResetLastPosition();
                }
            }
        }
    }
}
