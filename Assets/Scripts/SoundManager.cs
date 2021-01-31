using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioSource _source1;
	[SerializeField] private AudioSource _source2;
	[SerializeField] private AudioSource _oneShotSource;

	private AudioSource _current;
	private AudioSource _other;
	private object _tweenId = new object();

	public static SoundManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(this.gameObject);
		Setup();
	}

	private void Setup()
	{
		_current = _source1;
		_other = _source2;
		_source1.volume = 0f;
		_source2.volume = 0f;
	}

	private void OnDestroy()
	{
		DOTween.Kill(_tweenId);
		_tweenId = null;
	}

	public void PlayOneShot(AudioClip clip)
	{
		_oneShotSource.PlayOneShot(clip);
	}

	public void Stop()
	{
		DOTween.Kill(_tweenId);
		if (_current.isPlaying) _current.Stop();
		if (_other.isPlaying) _other.Stop();
	}

	public void CrossFade(AudioClip clip, float duration, float delay = 0f)
	{
		DOTween.Kill(_tweenId);
		if (!_current.isPlaying && !_other.isPlaying)
		{
			_current.clip = clip;
			_current.loop = true;
			_current.Play();
			DOVirtual
				.Float(0f, 1f, duration, val => { _current.volume = val; })
				.SetDelay(delay)
				.SetId(_tweenId);
		}
		else
		{
			_other = _current == _source1 ? _source2 : _source1;

			_other.clip = clip;
			_other.loop = true;
			_other.Play();

			DOVirtual
				.Float(0f, 1f, duration, val =>
				{
					_other.volume = val;
					_current.volume = 1f - val;
				})
				.SetDelay(delay)
				.OnComplete(() =>
				{
					_current.Stop();
					var temp = _current;
					_current = _other;
					_other = temp;
				})
				.SetId(_tweenId);
		}
	}
}
