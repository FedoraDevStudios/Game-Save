using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonGameSave : IGameSave
{
	[SerializeField, BoxGroup("Options")] string _saveDirectory = "Saves";
	[SerializeField, BoxGroup("Options")] string _fileType = "sav";
	[SerializeField, BoxGroup("Options")] Formatting _format;
	[SerializeField, BoxGroup("Options")] bool _encrypt = false;
	[SerializeField, HideLabel, BoxGroup("Game Data")] Dictionary<string, IGameData> _gameData = new Dictionary<string, IGameData>();
	[SerializeField, HideLabel, BoxGroup("Options/Encryptor"), ShowIf("_encrypt")] IEncrypt _encryptor;

	#region Editor Visuals
#if UNITY_EDITOR
	[ShowInInspector, HideLabel, BoxGroup("File Location")] string FileLocation => $"{Application.persistentDataPath}/{_saveDirectory}/filename.{_fileType}";
	[Button, BoxGroup("File Location")]
	void ShowFileLocation()
	{
		string dir = $"{Application.persistentDataPath}/{_saveDirectory}/";
		HandleMissingDirectories(dir);
		UnityEditor.EditorUtility.RevealInFinder(dir);
	}
#endif
	#endregion

	public void Save(string filename)
	{
		string filePath = GetFilePath(filename);
		HandleMissingDirectories(filePath);

		string jsonData = JsonConvert.SerializeObject(_gameData, _format);

		if (_encrypt)
			jsonData = _encryptor.Encrypt(jsonData);

		StreamWriter writer = new StreamWriter(filePath);
		writer.Write(jsonData);
		writer.Close();
	}

	public void Load(string filename)
	{
		string filePath = GetFilePath(filename);
		HandleMissingDirectories(filePath);

		StreamReader reader = new StreamReader(filePath);
		string jsonData = reader.ReadToEnd();
		reader.Close();

		if (_encrypt)
			jsonData = _encryptor.Decrypt(jsonData);

		Dictionary<string, IGameData> gameData = JsonConvert.DeserializeObject<Dictionary<string, IGameData>>(jsonData);

		foreach (KeyValuePair<string, IGameData> data in gameData)
		{
			if (_gameData.ContainsKey(data.Key))
				_gameData[data.Key].LoadData(data.Value);
			else
				Debug.LogError($"Unexpected data received from file. key '{data.Key}' was not found in the configured data layout.");
		}
	}

	string GetFilePath(string filename) => $"{Application.persistentDataPath}/{_saveDirectory}/{filename}.sav";

	void HandleMissingDirectories(string filePath)
	{
		string[] fileSplit = filePath.Split('/');
		string directory = fileSplit[0];

		for (int i = 1; i < fileSplit.Length - 1; i++)
		{
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);
			directory += $"/{fileSplit[i]}";
		}
	}
}
