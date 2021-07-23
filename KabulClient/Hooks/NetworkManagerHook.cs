using System;
using MelonLoader;
using VRC;
using VRC.Core;

namespace KabulClient.Hooks
{
    public static class NetworkManagerHook
    {
        private static bool isInitialized;
        private static bool seenFire;
        private static bool aFiredFirst;

        public static event Action<Player> OnJoin;
        public static event Action<Player> OnLeave;

        public static void EventHandlerA(Player player)
        {
            if (!seenFire)
            {
                aFiredFirst = true;
                seenFire = true;

                MelonDebug.Msg("A fired first");
            }

            if (player == null)
            {
                return;
            }

            (aFiredFirst ? OnJoin : OnLeave)?.Invoke(player);
        }

        public static void EventHandlerB(Player player)
        {
            if (!seenFire)
            {
                aFiredFirst = false;
                seenFire = true;

                MelonDebug.Msg("B fired first");
            }

            if (player == null)
            {
                return;
            }

            (aFiredFirst ? OnLeave : OnJoin)?.Invoke(player);
        }

        public static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            if (ReferenceEquals(NetworkManager.field_Internal_Static_NetworkManager_0, null))
            {
                return;
            }

            var field0 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0;
            var field1 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1;

            AddDelegate(field0, EventHandlerA);
            AddDelegate(field1, EventHandlerB);

            isInitialized = true;
        }

        private static void AddDelegate(VRCEventDelegate<Player> field, Action<Player> eventHandlerA)
        {
            field.field_Private_HashSet_1_UnityAction_1_T_0.Add(eventHandlerA);
        }
    }
}
