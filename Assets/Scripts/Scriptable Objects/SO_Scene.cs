using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scene", menuName = "SecretOS/New Scene")]
public class SO_Scene : ScriptableObject
{
	public string sceneName;
	[TextArea]
	public string scene;

	public string RenderScene(string history)
	{
		string render = "";

		foreach (string s in scene.Split('\n'))
		{
			Debug.Log(s);
		}

		return render;
	}
}
