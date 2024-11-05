using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Directory", menuName = "SecretOS/New Directory")]
public class SO_Directory : ScriptableObject
{
	public string directoryName;
	public List<SO_File> files = new();

	public List<SO_File> GetFilesAtPath(string path)
	{
		List<SO_File> retVal = new();
		foreach (SO_File file in files)
		{
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
			// If the path ends here we can continue
			if (file.path == path)
			{
				continue;
			}

			// if the first part of the path matches
			if (file.path.Substring(0, path.Length) == path)
			{
				// get just the part AFTER the current path
				string rem = file.path.Substring(path.Length);
				// get just the first folder
				rem = rem.Substring(0, rem.IndexOf('\\'));

				// only add it if it's not already there
				if (!retVal.Contains(rem))
				{
					retVal.Add(rem);
				}
			}
		}

		return retVal;
	}
}
