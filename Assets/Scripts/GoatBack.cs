using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatBack : MonoBehaviour
{
	[SerializeField] private Transform _mamaGoatAttachmentPoint;
	[SerializeField] private int _maxCarry;

	private Stack<GoatKid> _goatKids;

	public IReadOnlyCollection<GoatKid> GoatKids => _goatKids;

	public Transform TopAttachPoint =>
		_goatKids.Count == 0 ? _mamaGoatAttachmentPoint : _goatKids.Peek().AttachmentPoint;

	public bool HasSpace => _goatKids.Count < _maxCarry;

	private void Awake()
	{
		_goatKids = new Stack<GoatKid>();
	}

	private void OnDestroy()
	{
		_goatKids.Clear();
	}

	public void Attach(GoatKid goat)
	{
		_goatKids.Push(goat);
	}

	public void DetachAllKids(GoatPen pen)
	{
		// Do as animation and with more info to not detach them all in the same spot
		while (_goatKids.Count > 0)
		{
			_goatKids.Pop().Detach(pen);
		}
	}
}