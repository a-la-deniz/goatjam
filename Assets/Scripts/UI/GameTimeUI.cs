using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField] private GameController _game;
    [SerializeField] private TMP_Text _text;

    private void LateUpdate()
    {
        if (_game.IsGameOver) return;
        var span = TimeSpan.FromSeconds(_game.TimeLeft);
        var text = span.ToString("m':'ss':'ff");
        _text.SetText(text);
    }
}
