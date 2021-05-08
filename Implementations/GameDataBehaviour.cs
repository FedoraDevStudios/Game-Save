using Sirenix.OdinInspector;
using UnityEngine;

[HideMonoScript]
public class GameDataBehaviour : SerializedMonoBehaviour, IGameData
{
    [SerializeField, HideLabel, BoxGroup("Game Data")] IGameData _gameData;

    public IGameData GameData => _gameData;

    public void LoadData(byte[] data) => _gameData.LoadData(data);
    public byte[] SaveData() => _gameData.SaveData();
}
