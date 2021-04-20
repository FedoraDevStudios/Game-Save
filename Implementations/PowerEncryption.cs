using Sirenix.OdinInspector;
using System.Text;
using UnityEngine;

public class PowerEncryption : IEncrypt
{
	[SerializeField] int _key = 129;

	public string Decrypt(string encrypted) => Process(encrypted);
	public string Encrypt(string humanReadable) => Process(humanReadable);

	string Process(string input)
	{
		StringBuilder inString = new StringBuilder(input);
		StringBuilder outString = new StringBuilder(input.Length);
		char c;

		for (int i = 0; i < input.Length; i++)
		{
			c = inString[i];
			c = (char)(c ^ _key);
			outString.Append(c);
		}

		return outString.ToString();
	}
}
