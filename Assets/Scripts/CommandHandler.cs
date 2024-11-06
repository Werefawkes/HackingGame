using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
	public GameManager gameManager;

	public DisplayManager DisplayManager;
	public TMPro.TMP_InputField inputField;

	public delegate void CommandCallback(string[] arguments);

	public Dictionary<string, Command> commands = new();

	public struct Command
	{
		public string commandWord;
		public string syntax;
		public string description;
		public int minArguments;
		public int maxArguments;
		public CommandCallback callback;
		public void Invoke(string[] arguments)
		{
			callback(arguments);
		}
	}

	private void Start()
	{
		Command help = new()
		{
			commandWord = "help",
			syntax = "help [command]",
			description = "Lists available commands or info about a command",
			minArguments = 0,
			maxArguments = 1,
			callback = Command_Help
		};

		Command ls = new()
		{
			commandWord = "ls",
			syntax = "ls",
			description = "Lists folders and files at the current path",
			minArguments = 0,
			maxArguments = 0,
			callback = Command_List
		};

		Command cd = new()
		{
			commandWord = "cd",
			syntax = "cd [folder]",
			description = "Changes the current directory. Pass '..' to move up one directory",
			minArguments = 0,
			maxArguments = 1,
			callback = Command_ChangeDirectory
		};

		commands.Add("help", help);
		commands.Add("ls", ls);
		commands.Add("cd", cd);
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

		if (commands.TryGetValue(arguments[0], out Command command))
		{
			if (arguments.Length - 1 < command.minArguments)
			{
				DisplayManager.PrintLine($"Error: Too few arguments for command '{command.commandWord}'", "red");
			}
			else if (arguments.Length - 1 > command.maxArguments)
			{
				DisplayManager.PrintLine($"Error: Too many arguments for command '{command.commandWord}'", "red");
			}
			else
			{
				command.Invoke(arguments);
			}
		}
		else
		{
			DisplayManager.PrintLines($"<color=red>Unknown command \"{arguments[0]}\"</color>".Split('$'));
		}
	}


	void Command_Help(string[] arguments)
	{
		string HelpString = "";

		if (arguments.Length == 2)
		{
			if (commands.TryGetValue(arguments[1], out Command command))
			{
				HelpString += $"Usage: {command.syntax}${command.description}$";
			}
			else
			{
				DisplayManager.PrintLine("Unknown command: " + arguments[1], "red");
				return;
			}
		}
		else
		{
			// List commands
			HelpString += "Available commands:$";

			foreach (string s in commands.Keys)
			{
				HelpString += $"	* {s}$";
			}
		}

		DisplayManager.PrintLines(HelpString.Split('$'));
	}

	void Command_List(string[] arguments)
	{
		string listString = "Path " + GameManager.CurrentPath + ":$";

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

	void Command_ChangeDirectory(string[] arguments)
	{
		if (arguments.Length < 2)
		{
			gameManager.ResetPath();
			return;
		}

		string targetPath = GameManager.CurrentPath + arguments[1];
		if (!targetPath.EndsWith('\\'))
		{
			targetPath += '\\';
		}

		if (!gameManager.TryChangePath(targetPath))
		{
			DisplayManager.PrintLine($"Error: Path {targetPath} does not exist", "red");
		}
	}
}
