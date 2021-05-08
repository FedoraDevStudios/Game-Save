using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.GameSave.Implementations
{
    [HideMonoScript]
    public class GameSaveBehaviour : SerializedMonoBehaviour, IGameSave
    {
        [SerializeField, HideLabel, BoxGroup("Game Save")] IGameSave _gameSave;

        public void Save(string filename) => _gameSave.Save(filename);
        public void Load(string filename) => _gameSave.Load(filename);
    }
}