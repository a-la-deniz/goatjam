using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public enum GameState
	{
		None,
		Playing,
		Lost,
		Won
	}

	[SerializeField] private int _totalGoats;
	[SerializeField] private float _gameTime;

	public int TotalGoats => _totalGoats;
	public float TimeLeft => _countdown;
	private float _countdown;

	public GameState State { get; private set; }

	public event System.Action<GameState> OnStateChanged;

	private void Awake()
	{
		_countdown = _gameTime;
	}

	private void Start()
	{
		// maybe called from something else
		StartGame();
	}

	private void OnDestroy()
	{
		OnStateChanged = null;
	}

	private void Update()
	{
		if (State != GameState.Playing) return;
		_countdown -= Time.deltaTime;
		if (_countdown <= 0f)
		{
			Debug.Log("Game over, time up!");
			State = GameState.Lost;
			OnStateChanged?.Invoke(State);
		}
	}

	public void StartGame()
	{
		State = GameState.Playing;
		OnStateChanged?.Invoke(State);
	}

	public void WinGame()
	{
		if (State != GameState.Playing) return;
		Debug.Log("Game won!");
		State = GameState.Won;
		OnStateChanged?.Invoke(State);
	}
}
