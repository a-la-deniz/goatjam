using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMusic : MonoBehaviour
{
	[SerializeField] private AudioClip _introMusic;
	private void Start()
	{
		SoundManager.Instance.Stop();
		SoundManager.Instance.CrossFade(_introMusic, 0.1f);
	}
}
