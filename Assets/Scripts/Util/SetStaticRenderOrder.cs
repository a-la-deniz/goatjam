using UnityEngine;

namespace Util
{
	public class SetStaticRenderOrder : MonoBehaviour
	{
		private Renderer _renderer;

		// Start is called before the first frame update
		private void Awake()
		{
			_renderer = GetComponent<Renderer>();
			_renderer.sortingOrder = (int) (_renderer.bounds.min).y * -1;
		}
	}
}