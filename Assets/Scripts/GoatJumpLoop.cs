using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GoatJumpLoop : MonoBehaviour
{
	[SerializeField] private SpriteRenderer _jump;
	[SerializeField] private float _jumpHeight = 0.5f;
	[SerializeField] private Sprite _downSprite;
	[SerializeField] private Sprite _upSprite;

	private Rigidbody2D _rb;
	private object _tweenId = new object();


	private bool _wasFlipped;
	private bool _wasStopped;
	private float _originalPosition;
	private float _originalScaleX;

	private void Awake()
	{
		_originalPosition = _jump.transform.localPosition.y;
		_originalScaleX = _jump.transform.localScale.x;
		_rb = GetComponent<Rigidbody2D>();
	}

	private void OnDestroy()
	{
		DOTween.Kill(_tweenId);
		_tweenId = null;
	}

	private void LateUpdate()
	{
		var stopped = _rb.velocity.sqrMagnitude <= 0.1f;
		var stoppedOnX =  Mathf.Approximately(_rb.velocity.x, 0f);
		if (stopped || stoppedOnX) SetFlip(_wasFlipped);
		else SetFlip(_rb.velocity.x >= 0f);

		if (stopped && !_wasStopped)
		{
			DOTween.Kill(_tweenId);
			SetFlip(_wasFlipped);
			_jump.transform.DOLocalMoveY(_originalPosition, 0.1f).SetId(_tweenId);
		}
		else if (!stopped && _wasStopped)
		{
			DOTween.Kill(_tweenId);
			_jump.transform
				.DOLocalMoveY(_originalPosition + _jumpHeight, 0.1f)
				.SetEase(Ease.InOutFlash)
				.SetLoops(-1, LoopType.Yoyo)
				.SetId(_tweenId);
		}
		_wasStopped = stopped;
		_wasFlipped = _jump.transform.localScale.x < 0f;

		var up = _jump.transform.localPosition.y - _originalPosition > _jumpHeight * 0.5f;
		_jump.sprite = up ? _upSprite : _downSprite;
	}

	private void SetFlip(bool flip)
	{
		var scale = _jump.transform.localScale;
		scale.x = _originalScaleX * (flip ? -1f : 1f);
		_jump.transform.localScale = scale;
	}
}
