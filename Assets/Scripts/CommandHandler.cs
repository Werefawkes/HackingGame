using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
	public DisplayManager DisplayManager;
	public TMPro.TMP_InputField inputField;

	public void OnValueChanged(string value)
	{
		DisplayManager.UpdateCurrentText(value);
	}

	public void OnEndEdit(string value)
	{
		if (value.Trim() == "") return;

		DisplayManager.SendCurrentMessage();

		ParseCommand(value);

		inputField.text = "";
		inputField.ActivateInputField();
	}

	public void ParseCommand(string command)
	{
		string[] arguments = command.Split();

		if (arguments[0] == "help")
		{
			Command_Help();
		}
	}

	void Command_Help()
	{
		string HelpString = "Available commands:$$	help$	ls$	exit";

		DisplayManager.CommitMultiLineToHistory(HelpString.Split('$'));
	}
}
