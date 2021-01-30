using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatPen : MonoBehaviour
{
	[SerializeField] private GameController _game;
	[SerializeField] private int _goatsToWin;
	private int _returnedGoats;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var goatBack = collision.GetComponent<GoatBack>();
		if (goatBack == null) return;

		goatBack.DetachAllKids(this);
	}

	public void ReturnGoat()
	{
		_returnedGoats++;
		if (_returnedGoats >= _goatsToWin)
		{
			_game.WinGame();
		}
	}
}