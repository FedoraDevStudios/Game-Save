using Sirenix.OdinInspector;
using UnityEngine;

public class GameDataBehaviour : SerializedMonoBehaviour, IGameData
{
    [SerializeField, HideLabel, BoxGroup("Game Data")] IGameData _gameData;
}
