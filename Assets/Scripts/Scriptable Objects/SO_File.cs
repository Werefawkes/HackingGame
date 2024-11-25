using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New File", menuName = "SecretOS/New File")]
public class SO_File : ScriptableObject
{
	public string fileName;
	public string path;

	public virtual void RunFile()
	{
	}
}
