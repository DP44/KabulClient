using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRCSDK2;
using VRC;
using VRTK.Controllables.ArtificialBased;
using UnityEngine.UI;
using VRC.Core;
using MelonLoader;

namespace KabulClient.Features
{
    public struct AvatarData
    {
        public string asseturl;
        public int polys;
    }

    // TODO: Finish this.
    class AntiCrasher
    {
        public static Dictionary<string, AvatarData> antiCrashList = new Dictionary<string, AvatarData>();

        public static int GetPolyCount(GameObject player)
        {
            int polyCount = 0;
            var skinMeshs = player.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            foreach (var obj in skinMeshs)
            {
                if (obj != null)
                {
                    if (obj.sharedMesh == null)
                    {
                        continue;
                    }

                    polyCount += CountPolyMeshes(obj.sharedMesh);
                }
            }

            var meshFilters = player.GetComponentsInChildren<MeshFilter>(true);

            foreach (var obj in meshFilters)
            {
                if (obj != null)
                {
                    if (obj.sharedMesh == null)
                    {
                        continue;
                    }

                    polyCount += CountPolyMeshes(obj.sharedMesh);
                }
            }

            return polyCount;
        }

        internal static int CountPolys(Renderer r)
        {
            int polyCount = 0;
            SkinnedMeshRenderer meshRenderer = r as SkinnedMeshRenderer;

            if (meshRenderer != null)
            {
                if (meshRenderer.sharedMesh == null)
                {
                    return 0;
                }

                polyCount += CountPolyMeshes(meshRenderer.sharedMesh);
            }

            return polyCount;
        }

        private static int CountPolyMeshes(Mesh sourceMesh)
        {
            bool destroyCloneMesh = false;
            Mesh playerMesh;

            if (sourceMesh.isReadable)
            {
                playerMesh = sourceMesh;
            }
            else
            {
                // Copy the mesh.
                playerMesh = UnityEngine.Object.Instantiate<Mesh>(sourceMesh);
            }

            int polyMeshCount = 0;

            for (int i = 0; i < playerMesh.subMeshCount; i++)
            {
                polyMeshCount += playerMesh.GetTriangles(i).Length / 3;
            }

            if (destroyCloneMesh)
            {
                UnityEngine.Object.Destroy(playerMesh);
            }

            return polyMeshCount;
        }

        public static void DetectCrasher()
        {
            // Fetch a list of all the active users.
            var activeUsers = Utils.GetAllPlayers();

            if (activeUsers == null)
            {
                return;
            }

            foreach (var user in activeUsers)
            {
                // TODO: Detect crash avatars.
            }
        }
    }
}