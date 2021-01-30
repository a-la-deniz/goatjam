using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoatSpawner : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private GameObject _bushesParent;
	[SerializeField] private GameObject _goatKidPrefab;

	private void Awake()
	{
		var rnd = new System.Random();
		var bushes = _bushesParent
			.GetComponentsInChildren<Bush>()
			.OrderBy(user => rnd.Next()).Take(_game.TotalGoats).ToList();

		foreach (var bush in bushes)
		{
			var goat = Instantiate(_goatKidPrefab, bush.transform.position, Quaternion.identity, null).GetComponent<GoatKid>();
			goat.HideInBush(bush);
		}
	}
}
