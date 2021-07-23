using System.Linq;
using UnityEngine;
using VRCSDK2;

namespace KabulClient.Features
{
    class AntiPortal
    {
        public static bool antiPortalEnabled = false;

        public static void Toggle()
        {
            antiPortalEnabled = !antiPortalEnabled;
        }

        public static void Main()
        {
            if (antiPortalEnabled)
            {
                // Destroy all existing portals.
                // TODO: Test this.
                (from portal in Resources.FindObjectsOfTypeAll<PortalInternal>()
                 where portal.gameObject.activeInHierarchy && !portal.gameObject.GetComponentInParent<VRC_PortalMarker>()
                 select portal).ToList().ForEach(p =>
                {
                    UnityEngine.Object.Destroy(p.transform.root.gameObject);
                });
            }
        }
    }
}
