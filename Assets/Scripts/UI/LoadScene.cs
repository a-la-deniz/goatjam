using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	[SerializeField] private int _scene;

	public void Load()
	{
		SceneManager.LoadScene(_scene);
	}
}
