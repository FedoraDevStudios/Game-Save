using System;
using UnityEngine;

namespace FedoraDev.GameSave.Examples
{
	[Serializable]
	public class ExampleData : IGameData
	{
		[SerializeField] IByteManager _byteManager;
		[SerializeField] int _sampleInt;
		[SerializeField] float _sampleFloat;
		[SerializeField] string _sampleString;

		public void LoadData(byte[] data)
		{
			_byteManager.SetByteArray(data);
			_sampleInt = _byteManager.GetInt();
			_sampleFloat = _byteManager.GetFloat();
			_sampleString = _byteManager.GetString();
		}

		public byte[] SaveData()
		{
			_byteManager.SetByteArray();
			_byteManager.AddInt(_sampleInt);
			_byteManager.AddFloat(_sampleFloat);
			_byteManager.AddString(_sampleString);
			return _byteManager.GetByteArray();
		}
	}
}