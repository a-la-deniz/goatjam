using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomBaaaa : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
	[SerializeField] private List<string> _baaas;

	private void Awake()
	{
		_text.fontSize = Random.Range(4.5f, 7f);
		_text.SetText(_baaas[Random.Range(0, _baaas.Count)]);
		LayoutRebuilder.ForceRebuildLayoutImmediate(_text.transform as RectTransform);
	}
}
