using Sirenix.OdinInspector;
using UnityEngine;

public class GameDataBehaviour : SerializedMonoBehaviour, IGameData
{
    [SerializeField, HideLabel, BoxGroup("Game Data")] IGameData _gameData;

    public void LoadData(IGameData data) => _gameData.LoadData(data);
}
