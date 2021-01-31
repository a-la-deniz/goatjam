using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class ScreamVisualsProvider : MonoBehaviour
{
	[SerializeField] private RectTransform         _screamPrefab;
	[SerializeField] private float                 _timeOnScreen;
	private static           ScreamVisualsProvider _instance;

	private static ScreamVisualsProvider Instance
	{
		get
		{
			if (_instance != null) return _instance;

			_instance = FindObjectOfType<ScreamVisualsProvider>();
			return _instance;
		}
	}

	private static readonly Queue<Tuple<float, RectTransform>> Screams = new Queue<Tuple<float, RectTransform>>();


	public static void CreateScream(float clipX, float clipY)
	{
		var instance = Instance;
		var mainCamera = Camera.main;
		clipX = clipX * 0.9f;
		clipY = clipY * 0.9f;

		var responseOnViewport = new Vector3(clipX.Remap(-1, 1, 0, 1),
				clipY.Remap(-1, 1, 0, 1),
				10f);

		var responseOnWorld = mainCamera.ViewportToWorldPoint(responseOnViewport);

		var scream = Instantiate(instance._screamPrefab, responseOnWorld, Quaternion.identity, instance.transform);
		Screams.Enqueue(new Tuple<float, RectTransform>(Time.time, scream));
	}

	private void Update()
	{
		var time = Time.time;
		if(Screams.Count == 0) return;

		var peek = Screams.Peek();

		if (peek != null && time - peek.Item1 > _timeOnScreen)
		{
			var destroyScream = Screams.Dequeue();
			Destroy(destroyScream.Item2.gameObject);
		}
	}

	private void OnDestroy()
	{
		if (_instance == this)
		{
			Screams.Clear();
			_instance = null;
		}
	}
}