using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SO_Directory;

public class GameManager : MonoBehaviour
{
	public SO_Directory currentDirectory;
	public static string CurrentPath { get; private set; }

	private void Start()
	{
		ChangeDirectory(currentDirectory);
	}

	public void ChangeDirectory(SO_Directory newDirectory)
	{
		currentDirectory = newDirectory;
		CurrentPath = currentDirectory.rootPath;
	}

	public List<SO_File> GetCurrentFiles()
	{
		return currentDirectory.GetFilesAtPath(CurrentPath);
	}

	public List<string> GetCurrentFolders()
	{
		return currentDirectory.GetFoldersAtPath(CurrentPath);
	}

	public PathState DoesPathExist(string path)
	{
		return currentDirectory.DoesPathExist(path);
	}

	public bool TryChangePath(string path)
	{
		if (currentDirectory.DoesPathExist(path) == PathState.Exists)
		{
			CurrentPath = path;
			return true;
		}

		return false;
	}

	public SO_File GetFileAtPath(string path, string fileName)
	{
		SO_File file = currentDirectory.GetFilesAtPath(path).Find(f => f.fileName == fileName);
		return file;
	}

	public SO_File GetFileAtPath(string fileName)
	{
		return GetFileAtPath(CurrentPath, fileName);
	}

	public void ResetPath()
	{
		CurrentPath = currentDirectory.rootPath;
	}

	public static void ExitGame()
	{
		Application.Quit();
	}
}
