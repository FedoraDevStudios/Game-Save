using System;
using UnityEngine;

public class ExampleData : IGameData
{
    [SerializeField] int _sampleInt;
    [SerializeField] float _sampleFloat;
    [SerializeField] string _sampleString;

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
