using VRC.Udon;
using MelonLoader;
using UnityEngine;
using VRC.Udon.Common.Interfaces;
using Il2CppSystem.Collections.Generic;

namespace KabulClient
{
    class Udon
    {
        /// <summary>
        /// Fetches a list of all the GameObjects in the scene with the component attached.
        /// </summary>
        public static List<GameObject> GetUdonBehaviourGameObjects()
        {
            GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
            List<GameObject> udonBehaviours = new List<GameObject>();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<UdonBehaviour>() != null)
                {
                    udonBehaviours.Add(gameObject);
                }
            }

            return udonBehaviours;
        }

        /// <summary>
        /// Gets a list of events.
        /// </summary>
        /// <param name="udonBehaviour">The UdonBehaviour component to check.</param>
        public static List<KeyValuePair<string, List<uint>>> GetEvents(UdonBehaviour udonBehaviour)
        {
            List<KeyValuePair<string, List<uint>>> events = new List<KeyValuePair<string, List<uint>>>();

            // Sanity check.
            if (udonBehaviour == null)
            {
                return null;
            }

            // Iterate through all the events possible in the event table and try to send them.
            foreach (KeyValuePair<string, List<uint>> udonEvent in udonBehaviour?._eventTable)
            {
                events.Add(udonEvent);
            }

            return events;
        }

        /// <summary>
        /// Calls a UDON event.
        /// </summary>
        /// <param name="udonBehaviour">The UdonBehaviour component to call from.</param>
        /// <param name="eventName">The name of the event to call.</param>
        public static void CallUdonEvent(UdonBehaviour udonBehaviour, string eventName)
        {
            udonBehaviour?.SendCustomNetworkEvent(NetworkEventTarget.All, eventName);
        }
    }
}
