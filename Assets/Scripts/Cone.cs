using System;
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

	private void Awake()
	{
		var renderer = _spriteRoot.GetComponentInChildren<SpriteRenderer>(true);
		var material = renderer.material;
	}

	[ContextMenu("GetOverlap")]
	public void GetOverlap(List<Collider2D> results)
	{
		Physics2D.OverlapCollider(_polygonCollider2D, _contactFilter2D, results);
	}
}