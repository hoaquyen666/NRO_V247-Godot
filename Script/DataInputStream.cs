using System;
using System.Threading;
using Godot;

public class DataInputStream
{
	public myReader r;

	private const int INTERVAL = 5;

	private const int MAXTIME = 500;

	public static DataInputStream istemp;

	private static int status;

	private static string filenametemp;

	public DataInputStream(string filename)
	{
		// Normalize path - remove leading slash
		string normalizedFilename = filename;
		if (normalizedFilename.StartsWith("/"))
		{
			normalizedFilename = normalizedFilename.Substring(1);
		}
		
		// Build resource path - handle if path already contains "res/" prefix
		string resourcePath;
		if (normalizedFilename.StartsWith("res/"))
		{
			resourcePath = "res://Resources/" + normalizedFilename;
		}
		else
		{
			// Path doesn't have res/ prefix, add it
			resourcePath = "res://Resources/res/" + normalizedFilename;
		}
		
		// Try to open file with multiple extension fallbacks
		var file = FileAccess.Open(resourcePath, FileAccess.ModeFlags.Read);
		
		// If not found, try adding .bytes extension
		if (file == null && !resourcePath.EndsWith(".bytes"))
		{
			file = FileAccess.Open(resourcePath + ".bytes", FileAccess.ModeFlags.Read);
		}
		
		// If still not found with x2/x3/x4, try fallback to x1
		if (file == null && (resourcePath.Contains("/x2/") || resourcePath.Contains("/x3/") || resourcePath.Contains("/x4/")))
		{
			string fallbackPath = resourcePath.Replace("/x2/", "/x1/").Replace("/x3/", "/x1/").Replace("/x4/", "/x1/");
			file = FileAccess.Open(fallbackPath, FileAccess.ModeFlags.Read);
			if (file == null && !fallbackPath.EndsWith(".bytes"))
			{
				file = FileAccess.Open(fallbackPath + ".bytes", FileAccess.ModeFlags.Read);
			}
		}
		
		if (file != null)
		{
			byte[] bytes = file.GetBuffer((long)file.GetLength());
			r = new myReader(ArrayCast.cast(bytes));
			file.Close();
		}
		else
		{
			if (!resourcePath.Contains("/Mob/"))
			{
				GD.PushWarning("Failed to load file: " + resourcePath);
			}
			r = new myReader(new sbyte[0]);
		}
	}

	public DataInputStream(sbyte[] data)
	{
		r = new myReader(data);
	}

	public static void update()
	{
		if (status == 2)
		{
			status = 1;
			istemp = __getResourceAsStream(filenametemp);
			status = 0;
		}
	}

	public static DataInputStream getResourceAsStream(string filename)
	{
		return __getResourceAsStream(filename);
	}

	private static DataInputStream _getResourceAsStream(string filename)
	{
		if (status != 0)
		{
			for (int i = 0; i < 500; i++)
			{
				Thread.Sleep(5);
				if (status == 0)
				{
					break;
				}
			}
			if (status != 0)
			{
				GD.PushWarning("CANNOT GET INPUTSTREAM " + filename + " WHEN GETTING " + filenametemp);
				return null;
			}
		}
		istemp = null;
		filenametemp = filename;
		status = 2;
		int j;
		for (j = 0; j < 500; j++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (j == 500)
		{
			GD.PushWarning("TOO LONG FOR CREATE INPUTSTREAM " + filename);
			status = 0;
			return null;
		}
		return istemp;
	}

	private static DataInputStream __getResourceAsStream(string filename)
	{
		try
		{
			return new DataInputStream(filename);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public short readShort()
	{
		return r.readShort();
	}

	public int readInt()
	{
		return r.readInt();
	}

	public int read()
	{
		return r.readUnsignedByte();
	}

	public void read(ref sbyte[] data)
	{
		r.read(ref data);
	}

	public void close()
	{
		r.Close();
	}

	public void Close()
	{
		r.Close();
	}

	public string readUTF()
	{
		return r.readUTF();
	}

	public sbyte readByte()
	{
		return r.readByte();
	}

	public long readLong()
	{
		return r.readLong();
	}

	public bool readBoolean()
	{
		return r.readBoolean();
	}

	public int readUnsignedByte()
	{
		return (byte)r.readByte();
	}

	public int readUnsignedShort()
	{
		return r.readUnsignedShort();
	}

	public void readFully(ref sbyte[] data)
	{
		r.read(ref data);
	}

	public int available()
	{
		return r.available();
	}

	internal void read(ref sbyte[] byteData, int p, int size)
	{
		throw new NotImplementedException();
	}
}
