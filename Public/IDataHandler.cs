namespace FedoraDev.GameSave
{
    public interface IDataHandler
    {
        string ConvertToString<T>(T dataObject) where T : class;
        T ConvertToObject<T>(string dataString) where T : class;
    }
}