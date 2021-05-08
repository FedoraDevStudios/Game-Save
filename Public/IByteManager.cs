namespace FedoraDev.GameSave
{
    public interface IByteManager
    {
        void AddBool(bool boolean);
        void AddChar(char character);
        void AddString(string text, int maxBytes = 8);
        void AddFloat(float number);
        void AddFloat(double number);
        void AddInt(short number);
        void AddInt(int number);
        void AddInt(long number);
        void AddInt(ushort number);
        void AddInt(uint number);
        byte[] GetByteArray();
        void SetByteArray(byte[] bytes);

        bool GetBool();
        char GetChar();
        string GetString(int maxBytes = 8);
        float GetFloat();
        double GetDouble();
        short GetShort();
        int GetInt();
        long GetLong();
        ushort GetUShort();
        uint GetUInt();
    }
}