using Newtonsoft.Json;
using UnityEngine;

public class JsonDataHandler : IDataHandler
{
	[SerializeField] Formatting _formatting;

	public T ConvertToObject<T>(string dataString) where T : class
	{
		return JsonConvert.DeserializeObject<T>(dataString);
	}

	public string ConvertToString<T>(T dataObject) where T : class
	{
		return JsonConvert.SerializeObject(dataObject, _formatting);
	}
}
