using UnityEngine;

public class Cone : MonoBehaviour
{
	[SerializeField]private PolygonCollider2D _polygonCollider2D;
	[SerializeField]private Transform _spriteRoot;

	public PolygonCollider2D PolygonCollider2D => _polygonCollider2D;
	public Transform SpriteRoot => _spriteRoot;
}
