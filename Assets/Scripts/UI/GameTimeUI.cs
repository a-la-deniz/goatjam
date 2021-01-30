using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeUI : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private TMP_Text _text;

	private void Start()
	{
		UpdateVisual(_game.TimeLeft);
	}

	private void LateUpdate()
	{
		if (_game.State != GameController.GameState.Playing) return;
		UpdateVisual(_game.TimeLeft);
	}

	void UpdateVisual(float time)
	{
		var span = TimeSpan.FromSeconds(time);
		var text = span.ToString("m':'ss':'ff");
		_text.SetText(text);
	}
}
