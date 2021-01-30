using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _gameTime;

    private float _countdown;

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
            gameObject.SetActive(false);
        }
    }

    public void WinGame()
    {
        Debug.Log("Game won!");
    }
}
