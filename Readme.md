# Game Save

An extendable save system for Unity.

## Installation
This project uses [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041), which I cannot redistribute. If you don't own Odin Inspector, I would highly recommend purchasing it otherwise you won't be able to serialize interface instances as members which completely breaks this solution.

#### Package Manager
##### Git Extension
First, do yourself a favor and add the [UPM Git Extension](https://github.com/mob-sakai/UpmGitExtension) package to your project. This package makes git packages many times easier to use in your project. simply add `https://github.com/mob-sakai/UpmGitExtension.git` as a new package via the git option in the package manager. Afterwords, reopen the Package Manager.

##### Json.NET for Unity
Next, you will need to add [Json.NET for Unity](https://github.com/jilleJr/Newtonsoft.Json-for-Unity). In the top left, you will find a git logo. This button will show a small menu for adding git packages to your project. add `https://github.com/jilleJr/Newtonsoft.Json-for-Unity` in the `Repository URL` box and hit `Find Versions`. Select the latest `upm` version, i.e. `10.0.302 - upm` and then `Install Package`.

##### This Package
Finally, open the git menu again and add the URL for this repo. Select the version you wish to use and then `Install Package`.

#### UPM Upgrade
If you added the Git Extension package during installation, then you can change the installed version just like any other package.

#### Manual Installation
This can be added as a dependency to your Unity project manually. You just need to add a reference to this repo to your project's `Packages/manifest.json` file. Be sure to switch `[version]` with whichever release you would prefer, e.g. `.git#1.0.2`.

```js
{
    "dependencies": {
        ...,
        "com.fedoradev.developerconsole": "https://github.com/FedoraDevStudios/Game-Save.git#[version]"
    }
}
```

#### Manual Upgrade
After installing manually, you have to change both `Packages/manifest.json` and `Packages/packages-lock.json`. In the former, simply update the dependency with the version you wish to pull. In the lock file, you need to remove the entry for the package. This entry is a few lines long and everything needs to be deleted, including the curly braces. After this is successfully completed, moving back to Unity will force the application to download the desired version.

## Usage
### Add to Scene
For the easiest use, add a `GameSaveBehaviour` component to a game object in the scene. Then, select `DictionaryGameSave` in the drop down on the component. Next, select the `JsonDataHandler` in the `Data Handler` drop down. This can be changed later if you create your own handler. Finally, you can start adding entries in the `Game Data` dropdown. At first, you won't have anything to add until you create your own `IGameData` instances.

### Demo
As an example, go ahead and create a new object and add the `GameDataBehavour` component to it. You can select the data type from the `Game Data` drop down. For now, just use the `ExampleData` option and add any dummy values you wish to add. Next, in the `GameSaveBehaviour`, add an entry with any text as the key, then drag the `GameDataBehaviour` component into the value slot and hit the `Add` button. Finally, add a `TestingBehaviour` from the Example provided and assign the `GameSaveBehaviour` component to the `Game Save` slot. Hit `Save` and you'll find the save file is created. You can show the file in your file browser by clicking `Show File Location` in the `GameSaveBehaviour` > `File Options` drop down. Finish testing the functionality by changing some values in the data component and then hitting `Load` to see the values return to the saved ones.

### Explanation
Game Save is responsible for taking an arbitrary amount of data and storing it to disk. It will manage how and where that data is stored. It is important to note that this does not come for free and will require you to inform the system how the individual pieces of data will be organized. You do this by implementing `IGameData` in your object which will require you to convert the important parts of the object to and from a simplified byte array. You may want to brush up on [BitConverter](https://docs.microsoft.com/en-us/dotnet/api/system.bitconverter?view=net-5.0) as well as the size of each [Value Type](https://www.tutorialsteacher.com/csharp/csharp-data-typeshttps://docs.microsoft.com/en-us/dotnet/api/system.bitconverter?view=net-5.0) in C#.

#### IGameSave
This interface brings all of the others together. It stores a list of `IGameData` that gets run through the `IDataHandler` and optionally through `IEncrypt` before storing the data to a file on disk. You can implement your own version of this interface, however the included one comes with a lot of options.

#### IGameData
This interface should be implemented to enable an object to be saved to disk. This interface only requires 2 methods to be defined; `SaveData` and `LoadData`. Below are the implementations used in the `ExampleData` object in the included example. It is recommended to implement this in both the data object and the behaviour that contains it, passing the methods down to the relevant object.

##### SaveData
Here, we get each individual member and convert it to bytes. Note that `strings` can be any length and requires a bit extra work. After converting, we create a single byte array with just the right length and insert all of the bytes into it.
```C#
public byte[] SaveData()
{
	// Get Bytes from members
	byte[] sampleIntBytes = BitConverter.GetBytes(_sampleInt);
	byte[] sampleFloatBytes = BitConverter.GetBytes(_sampleFloat);
	List<byte[]> sampleStringBytes = new List<byte[]>();
	for (int i = 0; i < _sampleString.Length; i++)
		sampleStringBytes.Add(BitConverter.GetBytes(_sampleString[i]));

	// Create byte array with _just_ enough space
	int byteCount = sampleIntBytes.Length;
	byteCount += sampleFloatBytes.Length;
	byteCount += sampleStringBytes.Count * 2;
	byte[] bytes = new byte[byteCount];

	// Insert bytes into array
	for (int i = 0; i < sampleIntBytes.Length; i++)
		bytes[i] = sampleIntBytes[i];

	for (int i = 0; i < sampleFloatBytes.Length; i++)
	{
		int index = i + 4;
		bytes[index] = sampleFloatBytes[i];
	}

	for (int i = 0; i < sampleStringBytes.Count; i++)
	{
		int index = (i * 2) + 8;
		bytes[index] = sampleStringBytes[i][0];
		bytes[index + 1] = sampleStringBytes[i][1];
	}

	// Return array
	return bytes;
}
```

##### LoadData
When the data is loaded, all we need to do is convert the bytes back into their respective data types and store them back into the object's members.
Note that the data received should be in the same order that you define in SaveData()
```C#
public void LoadData(byte[] data)
{
	// Convert back to data types and assign back to object's members.
	_sampleInt = BitConverter.ToInt32(data, 0);
	_sampleFloat = BitConverter.ToSingle(data, 4);

	// Strings require a bit extra work. First make a char array, then join back to a string.
	char[] sampleStringCharacterBytes = new char[(data.Length - 8) / 2];

	for (int i = 0; i < sampleStringCharacterBytes.Length; i++)
	{
		int index = (i * 2) + 8;
		sampleStringCharacterBytes[i] = BitConverter.ToChar(data, index);
	}

	_sampleString = string.Join("", sampleStringCharacterBytes);
}
```

#### IDataHandler
This interface allows you to convert the data into a string that can be written to a file. The included implementation converts it to a JSON string, however you can create any type of handler for your data that you need.

#### IEncrypt
The final step before writing the data to a file. If encryption is turned on, you can select the type of encryption by using the dropdown in the Game Save object.