using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
	[SerializeField] private PolygonCollider2D _polygonCollider2D;
	[SerializeField] private Transform         _spriteRoot;

	public PolygonCollider2D PolygonCollider2D => _polygonCollider2D;
	public Transform         SpriteRoot        => _spriteRoot;

	private readonly ContactFilter2D _contactFilter2D = new ContactFilter2D()
														{
																useTriggers = true
														};

	[ContextMenu("GetOverlap")]
	private void GetOverlap()
	{
		var l = new List<Collider2D>();
		Physics2D.OverlapCollider(_polygonCollider2D, _contactFilter2D, l);
		foreach (var collider2D1 in l)
		{
			Debug.Log(collider2D1.name, collider2D1.gameObject);
		}
	}
}