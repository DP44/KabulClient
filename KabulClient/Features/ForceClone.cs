using VRC;
using VRC.UI;
using VRC.Core;
using MelonLoader;
using UnityEngine;

namespace KabulClient.Features
{
    class ForceClone
    {
        public static void CloneAvatar(Player selectedPlayer)
        {
            ApiAvatar avatar = selectedPlayer.prop_ApiAvatar_0;

            // Unfortunately if the avatar is private, you can't clone it.
            if (avatar.releaseStatus != "private")
            {
                Transform screens = GameObject.Find("UserInterface/MenuContent/Screens/")?.transform;
                PageAvatar avatarPage = screens.Find("Avatar")?.GetComponent<PageAvatar>();

                avatarPage.field_Public_SimpleAvatarPedestal_0.field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id };

                avatarPage?.ChangeToSelectedAvatar();
                MelonLogger.Msg($"Cloned player avatar. (ID: {avatar.id})");
            }
            else
            {
                MelonLogger.Error($"Attempted to clone private avatar! (ID: {avatar.id})");
            }
        }
    }
}
