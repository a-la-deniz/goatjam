using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cone))]
public class ConeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var cone = (Cone) target;
		var collider = cone.PolygonCollider2D;
		if (collider == null)
		{
			collider = cone.GetComponentInChildren<PolygonCollider2D>();
		}

		var pts = collider.points;

		if (pts.Length != 3)
		{
			pts = new[]
				  {
						  new Vector2(0, 0.0f),
						  new Vector2(0.32f, 0.64f),
						  new Vector2(-0.32f, 0.64f)
				  };
		}

		var va = pts[1] - pts[0];
		var vb = pts[2] - pts[0];
		var previousAngle = Vector2.Angle(vb, va);
		var angle = EditorGUILayout.FloatField("Cone Angle", previousAngle);
		angle = Mathf.Clamp(angle, 1, 179);

		var previousRange = (pts[0] - (pts[1] + pts[2]) * 0.5f).magnitude;
		var range = EditorGUILayout.FloatField("Cone Range", previousRange);
		range = Mathf.Clamp(range, 0.01f, 100f);

		pts[0] = new Vector2(0, 0);
		pts[1].x = range * Mathf.Tan(Mathf.Deg2Rad * angle * 0.5f);
		pts[1].y = pts[0].y + range;
		pts[2] = pts[1];
		pts[2].x = -pts[2].x;
		var defaultX = 0.64f;
		var defaultY = 0.64f;
		cone.SpriteRoot.localScale = new Vector3(pts[1].x * 2 / defaultX, range / defaultY, 1);
		collider.points = pts;

		if (previousAngle != angle || previousRange != range)
		{
			EditorUtility.SetDirty(collider);
			EditorUtility.SetDirty(target);
		}
	}
}