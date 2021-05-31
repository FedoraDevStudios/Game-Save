using System;
using System.Collections.Generic;

namespace FedoraDev.GameSave.Implementations
{
	public class ListByteManager : IByteManager
	{
		List<byte> _bytes = new List<byte>();

		List<byte> Bytes
		{
			get
			{
				if (_bytes == null)
					_bytes = new List<byte>();
				return _bytes;
			}

			set => _bytes = value;
		}

		public byte[] GetByteArray() => Bytes.ToArray();
		public void SetByteArray() => Bytes = new List<byte>();
		public void SetByteArray(byte[] bytes) => Bytes = new List<byte>(bytes);

		public void AddByte(byte data) => Bytes.Add(data);
		public void AddBool(bool boolean) => Bytes.AddRange(BitConverter.GetBytes(boolean));
		public void AddChar(char character) => Bytes.AddRange(BitConverter.GetBytes(character));
		public void AddFloat(float number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddFloat(double number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddInt(short number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddInt(int number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddInt(long number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddInt(ushort number) => Bytes.AddRange(BitConverter.GetBytes(number));
		public void AddInt(uint number) => Bytes.AddRange(BitConverter.GetBytes(number));

		public byte GetByte() => GetNextBytes(1)[0];
		public bool GetBool() => BitConverter.ToBoolean(GetNextBytes(1), 0);
		public char GetChar() => BitConverter.ToChar(GetNextBytes(2), 0);
		public double GetDouble() => BitConverter.ToDouble(GetNextBytes(8), 0);
		public float GetFloat() => BitConverter.ToSingle(GetNextBytes(4), 0);
		public int GetInt() => BitConverter.ToInt32(GetNextBytes(4), 0);
		public long GetLong() => BitConverter.ToInt64(GetNextBytes(8), 0);
		public short GetShort() => BitConverter.ToInt16(GetNextBytes(2), 0);
		public uint GetUInt() => BitConverter.ToUInt32(GetNextBytes(4), 0);
		public ushort GetUShort() => BitConverter.ToUInt16(GetNextBytes(2), 0);

		public void AddString(string text, int maxBytes = 32)
		{
			byte[] bytes = new byte[maxBytes];

			if (text.Length > maxBytes / 2)
				throw new OverflowException($"String length is too large to fit within {maxBytes} bytes. You can increase this limit by passing {text.Length * 2} or higher as a second argument in 'AddString'");

			for (int i = 0; i < text.Length; i++)
			{
				int index = i * 2;
				byte[] charBytes = BitConverter.GetBytes(text[i]);
				bytes[index] = charBytes[0];
				bytes[index + 1] = charBytes[1];
			}

			Bytes.AddRange(bytes);
		}

		public string GetString(int maxBytes = 32)
		{
			byte[] bytes = GetNextBytes(maxBytes);
			List<char> chars = new List<char>();

			for (int i = 0; i < maxBytes / 2; i++)
				chars.Add(BitConverter.ToChar(bytes, i * 2));
			return string.Join("", chars);
		}

		byte[] GetNextBytes(int quantity)
		{
			byte[] bytes = new byte[quantity];
			for (int i = 0; i < quantity; i++)
				bytes[i] = Bytes[i];
			for (int i = 0; i < quantity; i++)
				Bytes.RemoveAt(0);
			return bytes;
		}
	}
}