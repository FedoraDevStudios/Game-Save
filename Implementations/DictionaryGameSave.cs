using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FedoraDev.GameSave.Implementations
{
	[InlineProperty]
	public class DictionaryGameSave : IGameSave
	{
		[SerializeField, FoldoutGroup("File Options")] string _saveDirectory = "Saves";
		[SerializeField, FoldoutGroup("File Options")] string _fileType = "sav";
		[SerializeField, FoldoutGroup("File Options")] bool _encrypt = false;
		[SerializeField, FoldoutGroup("Data Handler"), HideLabel, InlineProperty] IDataHandler _dataHandler;
		[SerializeField, HideLabel, FoldoutGroup("Game Data")] Dictionary<string, IGameData> _gameData = new Dictionary<string, IGameData>();
		[SerializeField, HideLabel, FoldoutGroup("File Options/Encryptor"), ShowIf("_encrypt"), InlineProperty] IEncrypt _encryptor;

		#region Editor Visuals
#if UNITY_EDITOR
		[ShowInInspector, HideLabel, BoxGroup("File Options/File Location")] string FileLocation => $"{Application.persistentDataPath}/{_saveDirectory}/filename.{_fileType}";
		[Button, BoxGroup("File Options/File Location")]
		void ShowFileLocation()
		{
			string dir = $"{Application.persistentDataPath}/{_saveDirectory}/";
			Directory.CreateDirectory(GetFolderPath());
			UnityEditor.EditorUtility.RevealInFinder(dir);
		}
#endif
		#endregion

		public void Save(string filename)
		{
			string filePath = GetFilePath(filename);
			_ = Directory.CreateDirectory(GetFolderPath());

			Dictionary<string, byte[]> gameData = new Dictionary<string, byte[]>();

			foreach (KeyValuePair<string, IGameData> gd in _gameData)
				gameData.Add(gd.Key, gd.Value.SaveData());

			string jsonData = _dataHandler.ConvertToString(gameData);

			if (_encrypt)
				jsonData = _encryptor.Encrypt(jsonData);

			using (StreamWriter writer = new StreamWriter(filePath))
				writer.Write(jsonData);
		}

		public void Load(string filename)
		{
			string jsonData;
			string filePath = GetFilePath(filename);
			_ = Directory.CreateDirectory(GetFolderPath());

			using (StreamReader reader = new StreamReader(filePath))
				jsonData = reader.ReadToEnd();

			if (_encrypt)
				jsonData = _encryptor.Decrypt(jsonData);

			Dictionary<string, byte[]> gameData = _dataHandler.ConvertToObject<Dictionary<string, byte[]>>(jsonData);

			foreach (KeyValuePair<string, byte[]> data in gameData)
			{
				if (_gameData.ContainsKey(data.Key))
					_gameData[data.Key].LoadData(data.Value);
				else
					Debug.LogError($"Unexpected data received from file. key '{data.Key}' was not found in the configured data layout.");
			}
		}

		string GetFilePath(string filename) => $"{GetFolderPath()}/{filename}.sav";
		string GetFolderPath() => $"{Application.persistentDataPath}/{_saveDirectory}";
	}
}