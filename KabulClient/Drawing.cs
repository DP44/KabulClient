using UnityEngine;
using UnityEngine.Rendering;

namespace KabulClient
{
    class Drawing
    {
		/// <summary>
		/// Draws a line from two given points located in 3D space.
		/// </summary>
		/// <param name="src">The source position to begin drawing from.</param>
		/// <param name="dst">The destination to draw the line to.</param>
		/// <param name="lineColor">The color that the line will be.</param>
		/// <returns>The LineRenderer object.</returns>
		public static LineRenderer Create3DLine(Color lineColor)
		{
			// TODO: Find a proper way to render this over other objects.
			// https://stackoverflow.com/questions/19236482/how-to-create-a-line-using-two-vector3-points-in-unity
			GameObject lineObject = new GameObject("Line");

			// Just a test.
			lineObject.layer = 12;

			LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
			lineRenderer.startColor = lineColor;
			lineRenderer.endColor = Color.white;
			lineRenderer.startWidth = 0.01f;
			lineRenderer.endWidth = 0.01f;
			lineRenderer.positionCount = 2;
			lineRenderer.useWorldSpace = false;

			// Testing shit
			lineRenderer.rendererPriority = 0;
			// lineRenderer.renderingLayerMask = 4096; // 2 ^ 12 = 4096
			// lineRenderer.sortingLayerID = 12;
			// lineRenderer.material.renderQueue = (int)RenderQueue.Overlay;
			lineRenderer.sortingLayerName = "UiMenu";

			return lineRenderer;
		}
	}
}
