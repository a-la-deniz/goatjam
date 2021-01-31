using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private AudioClip _gameMusic;
	[SerializeField] private AudioClip _gameEndMusic;
	[SerializeField] private AudioClip _win;
	[SerializeField] private AudioClip _lose;

	private void Awake()
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
				SoundManager.Instance.Stop();
				SoundManager.Instance.PlayOneShot(_win);
				SoundManager.Instance.CrossFade(_gameEndMusic, 0.2f, 0.5f);
				break;
			case GameController.GameState.Lost:
				SoundManager.Instance.Stop();
				SoundManager.Instance.PlayOneShot(_lose);
				SoundManager.Instance.CrossFade(_gameEndMusic, 0.2f, 0.5f);
				break;
		}
	}
}
