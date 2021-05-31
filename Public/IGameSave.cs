namespace FedoraDev.GameSave
{
    public interface IGameSave
    {
        void Save(string filename);
        void Load(string filename);
    }
}