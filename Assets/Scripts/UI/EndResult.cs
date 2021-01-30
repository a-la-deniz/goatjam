using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndResult : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private TMP_Text _win;

	private void Start()
	{
		_game.OnStateChanged += OnStateChanged;
		gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (_game != null) _game.OnStateChanged -= OnStateChanged;
	}

	private void OnStateChanged(GameController.GameState state)
	{
		switch(state)
		{
			case GameController.GameState.Won:
				var span = TimeSpan.FromSeconds(_game.TimeLeft);
				var text = span.ToString("m':'ss':'ff");
				_win.text = $"You win with {text} time left!";
				gameObject.SetActive(true);
				break;
			case GameController.GameState.Lost:
				_win.text = $"You lose, boooooo!";
				gameObject.SetActive(true);
				break;
		}
	}
}
