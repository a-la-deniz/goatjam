using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	[SerializeField] private float _gameTime;

	public float TimeLeft => _countdown;
	private float _countdown;

	public bool IsGameOver { get; private set; }

	private void Awake()
	{
		_countdown = _gameTime;
	}

	private void Update()
	{
		_countdown -= Time.deltaTime;
		if (_countdown <= 0f)
		{
			Debug.Log("Game over, time up!");
			IsGameOver = true;
			gameObject.SetActive(false);
		}
	}

	public void WinGame()
	{
		Debug.Log("Game won!");
		IsGameOver = true;
		gameObject.SetActive(false);
	}
}
