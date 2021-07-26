using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC;
using VRC.UI;
using VRC.Core;
using MelonLoader;

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
                // Create a copy of the avatar as a pedestal.
                new PageAvatar
                {
                    field_Public_SimpleAvatarPedestal_0 = new SimpleAvatarPedestal { field_Internal_ApiAvatar_0 = new ApiAvatar { id = avatar.id }}
                }.ChangeToSelectedAvatar();

                MelonLogger.Msg($"Cloned player avatar. (ID: {avatar.id})");
            }
            else
            {
                MelonLogger.Error($"Attempted to clone private avatar! (ID: {avatar.id})");
            }
        }
    }
}
