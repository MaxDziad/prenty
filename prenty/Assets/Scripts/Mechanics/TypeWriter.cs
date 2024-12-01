using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TypeWriter : MonoBehaviour
{
	public event Action OnTypeEndedEvent;

	[SerializeField] Text text;
	[SerializeField] TMP_Text tmpProText;
	string writer;
	[SerializeField] private Coroutine coroutine;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = "";
	[SerializeField] bool leadingCharBeforeDelay = false;

	public bool IsTyping { get; private set; } = false;

	// Use this for initialization
	public void Awake()
	{
		if (text != null)
		{
			writer = text.text;
		}

		tmpProText = GetComponent<TextMeshProUGUI>();

		if (tmpProText != null)
		{
			writer = tmpProText.text;
		}
	}

	public void ClearText()
	{
		tmpProText.text = "";
	}

	public void StartTypewriter()
	{
		StopAllCoroutines();
		IsTyping = true;

		if (text != null)
		{
			text.text = "";

			StartCoroutine("TypeWriterText");
		}

		if (tmpProText != null)
		{
			tmpProText.text = "";

			StartCoroutine("TypeWriterTMP");
		}
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private IEnumerator TypeWriterText()
	{
		text.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSecondsRealtime(delayBeforeStart);

		foreach (char c in writer)
		{
			if (text.text.Length > 0)
			{
				text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
			}
			text.text += c;
			text.text += leadingChar;
			yield return new WaitForSecondsRealtime(timeBtwChars);
		}

		if (leadingChar != "")
		{
			text.text = text.text.Substring(0, text.text.Length - leadingChar.Length);
		}

		yield return null;
	}

	private IEnumerator TypeWriterTMP()
	{
		tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

		yield return new WaitForSecondsRealtime(delayBeforeStart);

		foreach (char c in writer)
		{
			if (tmpProText.text.Length > 0)
			{
				tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
			}
			tmpProText.text += c;
			tmpProText.text += leadingChar;
			yield return new WaitForSecondsRealtime(timeBtwChars);
		}

		if (leadingChar != "")
		{
			tmpProText.text = tmpProText.text.Substring(0, tmpProText.text.Length - leadingChar.Length);
		}

		IsTyping = false;
		OnTypeEndedEvent?.Invoke();
	}
}