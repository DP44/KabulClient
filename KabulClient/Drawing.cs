using System;
using UnityEngine;
using MelonLoader;

namespace KabulClient
{
    class Drawing
    {
        private static Material lineMaterial;

        public static void CreateLineMaterial()
        {
            if (lineMaterial != null)
            {
                MelonLogger.Warning("Tried to create line material when it exists!");
                return;
            }

            MelonLogger.Msg("Creating line material.");

            // lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            // Alpha blending.
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            // Disable backface culling.
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);

            // Disable depth writes.
            lineMaterial.SetInt("_ZWrite", 0);
        }

        /// <summary>
        /// Draws a line from two given points.
        /// </summary>
        /// <param name="src">The source position to begin drawing from.</param>
        /// <param name="dst">The destination to draw the line to.</param>
        /// <param name="lineColor">The color that the line will be.</param>
        public static void DrawLine(Vector2 src, Vector2 dst, Color lineColor)
        {
            // https://docs.unity3d.com/2018.4/Documentation/ScriptReference/GL.html
            if (lineMaterial == null)
            {
                MelonLogger.Error("lineMaterial is null!");
                return;
            }

            // Apply the line material.
            lineMaterial.SetPass(0);

            GL.PushMatrix();
            // GL.LoadOrtho();

            GL.Begin(1); // 1 is the const value for GL.LINES.
            GL.Color(lineColor);
            GL.Vertex(new Vector3(src.x, src.y, 0));
            GL.Vertex(new Vector3(dst.x, dst.y, 0));
            GL.End();

            GL.PopMatrix();
        }

        /// <summary>
        /// Draws a line between two points in world space.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="lineColor"></param>
        public static void Draw3DLine(Vector3 src, Vector3 dst, Color lineColor)
        {
            // TODO: Fix this entire function.

            if (lineMaterial == null)
            {
                MelonLogger.Error("lineMaterial is null!");
                return;
            }

            Camera localCamera = GameObject.Find("Camera (eye)")?.GetComponent<Camera>();

            if (localCamera == null)
            {
                MelonLogger.Msg("localCamera is null!");
                return;
            }

            // Calculate the world to screen point of the source position.
            Vector3 worldToScreenSrc = localCamera.WorldToScreenPoint(src);
            worldToScreenSrc.y = Screen.height - worldToScreenSrc.y;

            // Calculate the world to screen point of the destination position.
            Vector3 worldToScreenDst = localCamera.WorldToScreenPoint(dst);
            worldToScreenDst.y = Screen.height - worldToScreenDst.y;

            // Check for if both points are behind us.
            if (worldToScreenSrc.z <= 0 || worldToScreenDst.z <= 0)
            {
                return;
            }

            // Apply the line material.
            lineMaterial.SetPass(0);

            // Render the line.
            GL.PushMatrix();

            GL.Begin(1); // 1 is the const value for GL.LINES.
            GL.Color(lineColor);
            GL.Vertex(new Vector3(worldToScreenSrc.x, worldToScreenSrc.y, 0));
            GL.Vertex(new Vector3(worldToScreenDst.x, worldToScreenDst.y, 0));
            GL.End();

            GL.PopMatrix();
        }

        public static void DrawESPLine(GameObject player, Color lineColor)
        {
            if (lineMaterial == null)
            {
                MelonLogger.Error("lineMaterial is null!");
                return;
            }

            Camera localCamera = GameObject.Find("Camera (eye)")?.GetComponent<Camera>();

            if (localCamera == null)
            {
                MelonLogger.Msg("localCamera is null!");
                return;
            }

            Vector3 worldToScreenPos = localCamera.WorldToScreenPoint(player.transform.position);
            worldToScreenPos.y = Screen.height - worldToScreenPos.y;

            // Make sure the player isn't behind us, otherwise don't render the text.
            if (worldToScreenPos.z <= 0)
            {
                return;
            }

            // Apply the line material.
            lineMaterial.SetPass(0);

            // Render the line.
            GL.PushMatrix();

            GL.Begin(1); // 1 is the const value for GL.LINES.
            GL.Color(lineColor);
            GL.Vertex3(Screen.width / 2, Screen.height / 2, 0);
            GL.Vertex(new Vector3(worldToScreenPos.x, worldToScreenPos.y, 0));
            GL.End();

            GL.PopMatrix();
        }
    }
}
