using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
	public GameManager gameManager;

	string history;

	public string titleText;

	public string CurrentInput { get; private set; }

	public float caretBlinkRate = 0.85f;

	public const float LinePrintDelay = 0.15f;
	public const float LinePrintDelayDeviation = 0.1f;
	float linePrintTimer = 0;

	float caretInterval;
	float caretTimer = -100;

	public TMPro.TMP_Text displayText;
	public TMPro.TMP_InputField inputField;

	public List<string> lineQueue = new();

	private void Start()
	{
		caretInterval = 1f / caretBlinkRate;

		PrintLines(titleText.Split('$'));
	}

	private void Update()
	{
		if (lineQueue.Count > 0)
		{
			if (linePrintTimer <= 0)
			{
				PrintLine(lineQueue[0]);
				lineQueue.RemoveAt(0);
				linePrintTimer = LinePrintDelay + Random.Range(-LinePrintDelayDeviation, LinePrintDelayDeviation);
			}

			linePrintTimer -= Time.deltaTime;
		}

		string displayString = history;
		displayString += '\n' + GameManager.CurrentPath + "> " + CurrentInput;

		// Caret
		if (caretTimer < -caretInterval)
		{
			caretTimer = caretInterval;
		}

		if (caretTimer > 0)
		{
			displayString += '_';
		}
		caretTimer -= Time.deltaTime * 2;

		displayText.text = displayString;
	}

	public void UpdateCurrentText(string text)
	{
		CurrentInput = text;

		caretTimer = caretInterval;
	}

	public void SendCurrentMessage()
	{
		PrintLine(GameManager.CurrentPath + "> " + CurrentInput);
	}

	public void PrintLine(string message, string color = "white")
	{
		history += $"\n<color={color}>{message}</color>";
	}

	public void PrintLines(string[] messages)
	{
		lineQueue.AddRange(messages);
	}
}
