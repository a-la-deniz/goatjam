using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitOnEscape : MonoBehaviour
{
	[SerializeField] private bool _goToFirstScene;

	void Update()
    {
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (_goToFirstScene)
			{
				SceneManager.LoadScene(0);
			}
			else Application.Quit();
		}
    }
}
