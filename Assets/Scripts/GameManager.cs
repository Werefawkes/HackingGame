using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public SO_Directory currentDirectory;
	public string currentPath;

	public List<SO_File> GetCurrentFiles()
	{
		return currentDirectory.GetFilesAtPath(currentPath);
	}

	public List<string> GetCurrentFolders()
	{
		return currentDirectory.GetFoldersAtPath(currentPath);
	}
}
