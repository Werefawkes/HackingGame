using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
	public GameManager gameManager;

	static string history;

	//public SO_Scene currentScene;

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

	public static List<string> lineQueue = new();

	public static Vector2Int CharacterResolution = new(60, 30);

	private void Start()
	{
		caretInterval = 1f / caretBlinkRate;

		QueueLines(titleText.Split('$'));
		QueueLine("Type 'help' for a list of commands");
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

	public static void PrintLine(string message, string color = "white")
	{
		history += $"\n<color={color}>{message}</color>";
	}

	public static void QueueLines(string[] messages)
	{
		lineQueue.AddRange(messages);
	}

	public static void QueueLine(string message, string color = "white")
	{
		lineQueue.Add($"<color={color}>{message}</color>");
	}

	public static void PrintError(string message)
	{
		PrintLine("Error: " + message, "red");
	}
}
