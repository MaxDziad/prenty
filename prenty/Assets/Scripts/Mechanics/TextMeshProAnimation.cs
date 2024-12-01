using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProAnimation : MonoBehaviour
{
	public event Action OnSequenceEndEvent;

	[SerializeField]
	private float _oneCharTime = 0.1f;

	private TextMeshProUGUI _textMeshPro;

	private void Awake()
	{
		_textMeshPro = GetComponent<TextMeshProUGUI>();
	}

	public void StartTypingText()
	{
		StartCoroutine(MakeTextVisible());
	}

	private IEnumerator MakeTextVisible()
	{
		int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
		int counter = 0;

		while (true)
		{
			int visibleCount = counter % (totalVisibleCharacters + 1);
			_textMeshPro.maxVisibleCharacters = visibleCount;

			if (visibleCount >= totalVisibleCharacters)
			{
				break;
			}

			counter += 1;
			yield return new WaitForSecondsRealtime(_oneCharTime);
		}

		OnSequenceEndEvent?.Invoke();
	}
}
