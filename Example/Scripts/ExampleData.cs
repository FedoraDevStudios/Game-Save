using Newtonsoft.Json;
using System;
using UnityEngine;

[Serializable]
public class ExampleData : IGameData
{
    [SerializeField, JsonProperty] int _sampleInt;
    [SerializeField, JsonProperty] float _sampleFloat;
    [SerializeField, JsonProperty] string _sampleString;

    public void LoadData(IGameData data)
	{
		ExampleData eData = data as ExampleData;

		if (eData != null)
		{
			_sampleInt = eData._sampleInt;
			_sampleFloat = eData._sampleFloat;
			_sampleString = eData._sampleString;
		}
	}
}
