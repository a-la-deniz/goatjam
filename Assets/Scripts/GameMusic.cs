using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private AudioClip _gameMusic;
	[SerializeField] private AudioClip _gameEndMusic;

	private void Start()
	{
		_game.OnStateChanged += OnStateChanged;
	}

	private void OnDestroy()
	{
		if (_game != null) _game.OnStateChanged -= OnStateChanged;
	}

	private void OnStateChanged(GameController.GameState state)
	{
		switch (state)
		{
			case GameController.GameState.Playing:
				SoundManager.Instance.CrossFade(_gameMusic, 0.2f);
				break;
			case GameController.GameState.Won:
			case GameController.GameState.Lost:
				SoundManager.Instance.CrossFade(_gameEndMusic, 0.2f);
				break;
		}
	}
}
