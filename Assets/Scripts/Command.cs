using System;
using static CommandHandler;

public struct Command
{
	public string CommandWord;
	public string Syntax;
	public string Description;
	public int MinArguments;
	public int MaxArguments;
	public CommandCallback Callback;

	public Command(string commandWord, string syntax, string description, int minArguments, int maxArguments, CommandCallback callback)
	{
		CommandWord = commandWord;
		Syntax = syntax;
		Description = description;
		MinArguments = minArguments;
		MaxArguments = maxArguments;
		Callback = callback;
	}
	
	public bool IsValid()
	{
		return CommandWord != null && Callback != null;
	}
	public void Invoke(string[] arguments)
	{
		Callback(arguments);
	}
}
