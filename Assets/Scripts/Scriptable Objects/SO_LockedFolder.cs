using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Locked Folder", menuName = "SecretOS/New Locked Folder")]
public class SO_LockedFolder : SO_File
{
	public bool isLocked = true;
	public string password;

	public SO_Directory directory;
	public List<SO_File> lockedFiles = new();

	public bool TryUnlock(string passInput)
	{
		if (passInput == password)
		{
			directory.UnlockFiles(lockedFiles);
			return true;
		}

		return false;
	}
}
