using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text File", menuName = "SecretOS/New Text File")]
public class SO_TextFile : SO_File
{
	public string contents;

	public override void RunFile()
	{
		DisplayManager.QueueLine(path + fileName + ":");
		DisplayManager.QueueLine(contents + "\n");
	}
}