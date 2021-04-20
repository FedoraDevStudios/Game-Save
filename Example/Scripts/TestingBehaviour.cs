using Sirenix.OdinInspector;
using UnityEngine;

public class TestingBehaviour : SerializedMonoBehaviour
{
    [SerializeField] string _filename = "testingfile";
    [SerializeField] IGameSave _gameSave;

    [Button("Save")]
    public void Save() => _gameSave.Save(_filename);

    [Button("Load")]
    public void Load() => _gameSave.Load(_filename);
}
