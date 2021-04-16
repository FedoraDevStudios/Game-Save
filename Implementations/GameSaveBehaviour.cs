using Sirenix.OdinInspector;
using UnityEngine;

public class GameSaveBehaviour : SerializedMonoBehaviour, IGameSave
{
    [SerializeField, HideLabel, BoxGroup("Game Save")] IGameSave _gameSave;
    [SerializeField, HideLabel, BoxGroup("Game Data")] IGameData[] _gameData = new IGameData[0];
}
