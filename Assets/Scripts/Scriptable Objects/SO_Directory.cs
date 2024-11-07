using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Directory", menuName = "SecretOS/New Directory")]
public class SO_Directory : ScriptableObject
{
	public string directoryName;
	public string rootPath = "C:\\";
	public List<SO_File> files = new();

	public enum PathState
	{
		Exists,
		Locked,
		NotExists
	}
	public void UnlockFiles(List<SO_File> files)
	{

	}

	public List<SO_File> GetFilesAtPath(string path)
	{
		List<SO_File> retVal = new();
		foreach (SO_File file in files)
		{
			if (file.GetType() == typeof(SO_LockedFolder))
			{
				continue;
			}

			if (file.path == path)
			{
				retVal.Add(file);
			}
		}

		return retVal;
	}

	public List<string> GetFoldersAtPath(string path)
	{
		List<string> retVal = new();

		foreach (SO_File file in files)
		{
			if (file.path == path)
			{
				// If the file is a locked folder, add it
				if (file.GetType() == typeof(SO_LockedFolder))
				{
					retVal.Add(file.fileName + " [LOCKED]");
				}
				// If the path matches, there are no more folders to check
				continue;
			}

			// If the path ends here we can continue
			if (file.path.Length < path.Length)
			{
				continue;
			}

			// if the first part of the path matches
			if (file.path.Substring(0, path.Length) == path)
			{
				// get just the part AFTER the current path
				string rem = file.path.Substring(path.Length);
				// get just the first folder
				rem = rem.Substring(0, rem.IndexOf('\\') + 1);

				// only add it if it's not already there
				if (!retVal.Contains(rem))
				{
					retVal.Add(rem);
				}
			}
		}

		return retVal;
	}

	public PathState DoesPathExist(string path)
	{
		foreach (SO_File file in files)
		{
			if (file.path == path || file.path.Contains(path))
			{
				return PathState.Exists;
			}

			if (file.GetType() == typeof(SO_LockedFolder) && file.path + file.fileName == path)
			{
				return PathState.Locked;
			}
		}
		return PathState.NotExists;
	}
}
