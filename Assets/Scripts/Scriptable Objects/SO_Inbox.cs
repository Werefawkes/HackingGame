using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Inbox", menuName = "SecretOS/New Inbox")]
public class SO_Inbox : SO_File
{
	public string inboxEmailAddress;
	public List<Email> emails = new();

	[System.Serializable]
	public struct Email
	{
		public string subject;
		public string fromAddress;
		[TextArea]
		public string content;
	}

	public override void RunFile()
	{
		
	}
}