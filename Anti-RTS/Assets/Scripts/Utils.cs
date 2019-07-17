using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Utils
{
	public static T loadFromDisk<T>(string path)
	{
		using (Stream stream = File.Open(path, FileMode.Open))
		{
			return (T)new BinaryFormatter().Deserialize(stream);
		}
	}

	public static void saveToDisk(string path, Object obj)
	{
		using (Stream stream = File.Open(path, FileMode.Create))
		{
			new BinaryFormatter().Serialize(stream, obj);
		}
	}
}
