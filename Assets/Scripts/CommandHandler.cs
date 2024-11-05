using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
	public GameManager gameManager;

	public DisplayManager DisplayManager;
	public TMPro.TMP_InputField inputField;

	public delegate void CommandCallback(string[] arguments);

	public Dictionary<string, CommandCallback> commands = new();

	public struct Command
	{
		public string commandWord;
		public string syntax;
	}

	private void Start()
	{
		CommandCallback help = Command_Help;
		commands.Add("help", help);
		CommandCallback ls = Command_List;
		commands.Add("ls", ls);
	}

	public void OnValueChanged(string value)
	{
		DisplayManager.UpdateCurrentText(value);
	}

	public void OnEndEdit(string value)
	{
		inputField.ActivateInputField();

		DisplayManager.SendCurrentMessage();

		if (value.Trim() != "")
		{
			ParseCommand(value.Trim());
		}

		inputField.text = "";
	}

	public void ParseCommand(string input)
	{
		string[] arguments = input.Split();

		if (commands.TryGetValue(arguments[0], out CommandCallback retcom))
		{
			retcom(arguments);
		}
		else
		{
			DisplayManager.PrintLines($"<color=red>Unknown command \"{arguments[0]}\"</color>".Split('$'));
		}
	}

	void Command_Help(string[] arguments)
	{
		string HelpString = "Available commands:$";

		foreach (string s in commands.Keys)
		{
			HelpString += $"	* {s}$";
		}

		DisplayManager.PrintLines(HelpString.Split('$'));
	}

	void Command_List(string[] arguments)
	{
		string listString = "Path " + gameManager.currentPath + ":$";

		List<string> folders = gameManager.GetCurrentFolders();

		foreach (string folder in folders)
		{
			listString += $"	* <color=purple>{folder}\\</color>$";
		}

		List<SO_File> files = gameManager.GetCurrentFiles();

		foreach (SO_File file in files)
		{
			listString += $"	* {file.fileName}$";
		}

		DisplayManager.PrintLines(listString.Split('$'));
	}
}
