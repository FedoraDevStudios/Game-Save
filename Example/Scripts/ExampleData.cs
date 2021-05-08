using System;
using System.Collections.Generic;
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

			/*
			_sampleInt = BitConverter.ToInt32(data, 0);
			_sampleFloat = BitConverter.ToSingle(data, 4);

			char[] sampleStringCharacterBytes = new char[(data.Length - 8) / 2];

			for (int i = 0; i < sampleStringCharacterBytes.Length; i++)
			{
				int index = (i * 2) + 8;
				sampleStringCharacterBytes[i] = BitConverter.ToChar(data, index);
			}

			_sampleString = string.Join("", sampleStringCharacterBytes);
			*/
		}

		public byte[] SaveData()
		{
			_byteManager.AddInt(_sampleInt);
			_byteManager.AddFloat(_sampleFloat);
			_byteManager.AddString(_sampleString);
			return _byteManager.GetByteArray();
			/*
			byte[] sampleIntBytes = BitConverter.GetBytes(_sampleInt);
			byte[] sampleFloatBytes = BitConverter.GetBytes(_sampleFloat);
			List<byte[]> sampleStringBytes = new List<byte[]>();
			for (int i = 0; i < _sampleString.Length; i++)
				sampleStringBytes.Add(BitConverter.GetBytes(_sampleString[i]));

			int byteCount = sampleIntBytes.Length;
			byteCount += sampleFloatBytes.Length;
			byteCount += sampleStringBytes.Count * 2;
			byte[] bytes = new byte[byteCount];

			for (int i = 0; i < sampleIntBytes.Length; i++)
				bytes[i] = sampleIntBytes[i];

			for (int i = 0; i < sampleFloatBytes.Length; i++)
			{
				int index = i + 4;
				bytes[index] = sampleFloatBytes[i];
			}

			for (int i = 0; i < sampleStringBytes.Count; i++)
			{
				int index = (i * 2) + 8;
				bytes[index] = sampleStringBytes[i][0];
				bytes[index + 1] = sampleStringBytes[i][1];
			}

			return bytes;
			*/
		}
	}
}