using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHandler : MonoBehaviour
{
	public GameManager gameManager;

	public DisplayManager DisplayManager;
	public TMPro.TMP_InputField inputField;

	public delegate void CommandCallback(string[] arguments);

	public List<Command> currentCommands = new();
	public List<Command> standardCommands = new();

	private void Start()
	{
		Command help = new()
		{
			CommandWord = "help",
			Syntax = "help [command]",
			Description = "Lists available commands or info about a command",
			MinArguments = 0,
			MaxArguments = 1,
			Callback = Command_Help
		};

		Command ls = new()
		{
			CommandWord = "ls",
			Syntax = "ls",
			Description = "Lists folders and files at the current path",
			MinArguments = 0,
			MaxArguments = 0,
			Callback = Command_List
		};

		Command cd = new()
		{
			CommandWord = "cd",
			Syntax = "cd [folder]",
			Description = "Changes the current directory. Pass '..' to move up one directory",
			MinArguments = 0,
			MaxArguments = 1,
			Callback = Command_ChangeDirectory
		};

		standardCommands.Add(help);
		standardCommands.Add(ls);
		standardCommands.Add(cd);

		currentCommands = standardCommands;
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

	public void YesNoPopup(string message, CommandCallback yesCallback, CommandCallback noCallback)
	{
		Command yes = new()
		{
			CommandWord = "yes",
			Callback = yesCallback
		};
		Command no = new()
		{
			CommandWord = "no",
			Callback = noCallback
		};

		Popup(message, new[] { yes, no } );
	}

	public void Popup(string message, Command[] responses)
	{
		// Send the message to the display
		DisplayManager.PrintLine(message);
	}

	#region Commands
	public void ParseCommand(string input)
	{
		string[] arguments = input.Split();

		Command command = currentCommands.Find(c => c.CommandWord == arguments[0]);
		
		if (command.IsValid())
		{
			if (arguments.Length - 1 < command.MinArguments)
			{
				DisplayManager.PrintLine($"Error: Too few arguments for command '{command.CommandWord}'", "red");
			}
			else if (arguments.Length - 1 > command.MaxArguments)
			{
				DisplayManager.PrintLine($"Error: Too many arguments for command '{command.CommandWord}'", "red");
			}
			else
			{
				command.Invoke(arguments);
			}
		}
		else
		{
			DisplayManager.QueueLines($"<color=red>Unknown command \"{arguments[0]}\"</color>".Split('$'));
		}
	}


	void Command_Help(string[] arguments)
	{
		string HelpString = "";

		if (arguments.Length == 2)
		{
			Command command = currentCommands.Find(c => c.CommandWord == arguments[1]);

			if (command.IsValid())
			{
				HelpString += $"Usage: {command.Syntax}${command.Description}$";
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

			foreach (Command c in currentCommands)
			{
				HelpString += $"	* {c.CommandWord}$";
			}
		}

		DisplayManager.QueueLines(HelpString.Split('$'));
	}

	void Command_List(string[] arguments)
	{
		string listString = "Path " + GameManager.CurrentPath + ":$";

		List<string> folders = gameManager.GetCurrentFolders();

		foreach (string folder in folders)
		{
			listString += $"	* <color=purple>{folder}</color>$";
		}

		List<SO_File> files = gameManager.GetCurrentFiles();

		foreach (SO_File file in files)
		{
			listString += $"	* {file.fileName}$";
		}

		DisplayManager.QueueLines(listString.Split('$'));
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
	#endregion
}
