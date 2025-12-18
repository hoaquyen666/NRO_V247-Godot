using System;
using System.Threading;
using Godot;

public class Image
{
	private const int INTERVAL = 5;

	private const int MAXTIME = 500;

	public ImageTexture texture;
	public Godot.Image rawImage;

	public static Image imgTemp;

	public static string filenametemp;

	public static byte[] datatemp;

	public static Image imgSrcTemp;

	public static int xtemp;

	public static int ytemp;

	public static int wtemp;

	public static int htemp;

	public static int transformtemp;

	public int w;

	public int h;

	public static int status;

	public Color colorBlend = new Color(0, 0, 0, 1);

	public Image()
	{
		rawImage = Godot.Image.CreateEmpty(1, 1, false, Godot.Image.Format.Rgba8);
		texture = ImageTexture.CreateFromImage(rawImage);
	}

	public static Image createEmptyImage()
	{
		return __createEmptyImage();
	}

	public static Image createImage(string filename)
	{
		return __createImage(filename);
	}

	public static Image createImage(byte[] imageData)
	{
		return __createImage(imageData);
	}

	public static Image createImage(Image src, int x, int y, int w, int h, int transform)
	{
		return __createImage(src, x, y, w, h, transform);
	}

	public static Image createImage(int w, int h)
	{
		return __createImage(w, h);
	}

	public static Image createImage(Image img)
	{
		Image image = createImage(img.w, img.h);
		image.rawImage = img.rawImage.Duplicate() as Godot.Image;
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		return image;
	}

	public static Image createImage(sbyte[] imageData, int offset, int lenght)
	{
		if (offset + lenght > imageData.Length)
		{
			return null;
		}
		byte[] array = new byte[lenght];
		for (int i = 0; i < lenght; i++)
		{
			array[i] = convertSbyteToByte(imageData[i + offset]);
		}
		return createImage(array);
	}

	public static byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
		{
			return (byte)var;
		}
		return (byte)(var + 256);
	}

	public static byte[] convertArrSbyteToArrByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}

	public static Image createRGBImage(int[] rbg, int w, int h, bool bl)
	{
		Image image = createImage(w, h);
		for (int j = 0; j < h; j++)
		{
			for (int i = 0; i < w; i++)
			{
				Color c = setColorFromRBG(rbg[j * w + i]);
				image.rawImage.SetPixel(i, j, c);
			}
		}
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		return image;
	}

	public static Color setColorFromRBG(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		float b = (float)num / 256f;
		float g = (float)num2 / 256f;
		float r = (float)num3 / 256f;
		return new Color(r, g, b);
	}

	public static void update()
	{
		if (status == 2)
		{
			status = 1;
			imgTemp = __createEmptyImage();
			status = 0;
		}
		else if (status == 3)
		{
			status = 1;
			imgTemp = __createImage(filenametemp);
			status = 0;
		}
		else if (status == 4)
		{
			status = 1;
			imgTemp = __createImage(datatemp);
			status = 0;
		}
		else if (status == 5)
		{
			status = 1;
			imgTemp = __createImage(imgSrcTemp, xtemp, ytemp, wtemp, htemp, transformtemp);
			status = 0;
		}
		else if (status == 6)
		{
			status = 1;
			imgTemp = __createImage(wtemp, htemp);
			status = 0;
		}
	}

	private static Image _createEmptyImage()
	{
		if (status != 0)
		{
			Cout.LogError("CANNOT CREATE EMPTY IMAGE WHEN CREATING OTHER IMAGE");
			return null;
		}
		imgTemp = null;
		status = 2;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Cout.LogError("TOO LONG FOR CREATE EMPTY IMAGE");
			status = 0;
		}
		return imgTemp;
	}

	private static Image _createImage(string filename)
	{
		if (status != 0)
		{
			Cout.LogError("CANNOT CREATE IMAGE " + filename + " WHEN CREATING OTHER IMAGE");
			return null;
		}
		imgTemp = null;
		filenametemp = filename;
		status = 3;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Cout.LogError("TOO LONG FOR CREATE IMAGE " + filename);
			status = 0;
		}
		return imgTemp;
	}

	private static Image _createImage(byte[] imageData)
	{
		if (status != 0)
		{
			Cout.LogError("CANNOT CREATE IMAGE(FromArray) WHEN CREATING OTHER IMAGE");
			return null;
		}
		imgTemp = null;
		datatemp = imageData;
		status = 4;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Cout.LogError("TOO LONG FOR CREATE IMAGE(FromArray)");
			status = 0;
		}
		return imgTemp;
	}

	private static Image _createImage(Image src, int x, int y, int w, int h, int transform)
	{
		if (status != 0)
		{
			Cout.LogError("CANNOT CREATE IMAGE(FromSrcPart) WHEN CREATING OTHER IMAGE");
			return null;
		}
		imgTemp = null;
		imgSrcTemp = src;
		xtemp = x;
		ytemp = y;
		wtemp = w;
		htemp = h;
		transformtemp = transform;
		status = 5;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Cout.LogError("TOO LONG FOR CREATE IMAGE(FromSrcPart)");
			status = 0;
		}
		return imgTemp;
	}

	private static Image _createImage(int w, int h)
	{
		if (status != 0)
		{
			Cout.LogError("CANNOT CREATE IMAGE(w,h) WHEN CREATING OTHER IMAGE");
			return null;
		}
		imgTemp = null;
		wtemp = w;
		htemp = h;
		status = 6;
		int i;
		for (i = 0; i < 500; i++)
		{
			Thread.Sleep(5);
			if (status == 0)
			{
				break;
			}
		}
		if (i == 500)
		{
			Cout.LogError("TOO LONG FOR CREATE IMAGE(w,h)");
			status = 0;
		}
		return imgTemp;
	}

	public static byte[] loadData(string filename)
	{
		string normalizedFilename = filename;
		if (normalizedFilename.StartsWith("/"))
		{
			normalizedFilename = normalizedFilename.Substring(1);
		}
		
		string resourcePath;
		if (normalizedFilename.StartsWith("res/"))
		{
			resourcePath = "res://Resources/" + normalizedFilename;
		}
		else
		{
			resourcePath = "res://Resources/res/" + normalizedFilename;
		}
		
		if (!resourcePath.EndsWith(".png") && !resourcePath.EndsWith(".jpg") && !resourcePath.EndsWith(".bytes"))
		{
			// Try common extensions
			if (ResourceLoader.Exists(resourcePath + ".bytes"))
			{
				resourcePath += ".bytes";
			}
			else if (ResourceLoader.Exists(resourcePath + ".png"))
			{
				resourcePath += ".png";
			}
		}
		
		if (!FileAccess.FileExists(resourcePath))
		{
			throw new Exception("NULL POINTER EXCEPTION AT Image loadData " + filename);
		}
		
		using var file = FileAccess.Open(resourcePath, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			throw new Exception("NULL POINTER EXCEPTION AT Image loadData " + filename);
		}
		byte[] data = file.GetBuffer((long)file.GetLength());
		GD.PrintErr("CHIEU DAI MANG BYTE IMAGE CREAT = " + data.Length);
		return data;
	}

	private static Image __createImage(string filename)
	{
		Image image = new Image();
		
		// Normalize path - remove leading slash if present
		string normalizedFilename = filename;
		if (normalizedFilename.StartsWith("/"))
		{
			normalizedFilename = normalizedFilename.Substring(1);
		}
		
		// Build resource path - handle if filename already starts with "res/"
		string resourcePath;
		if (normalizedFilename.StartsWith("res/"))
		{
			// Path already contains res/ from GameCanvas.loadImage etc.
			resourcePath = "res://Resources/" + normalizedFilename;
		}
		else
		{
			// Path doesn't have res/ prefix, add it
			resourcePath = "res://Resources/res/" + normalizedFilename;
		}
		
		// Build list of paths to try
		var pathsToTry = new System.Collections.Generic.List<string>();
		pathsToTry.Add(resourcePath);
		
		// If path contains x2, x3, x4 etc, add fallback to x1
		if (resourcePath.Contains("/x2/") || resourcePath.Contains("/x3/") || resourcePath.Contains("/x4/"))
		{
			string fallbackPath = resourcePath.Replace("/x2/", "/x1/").Replace("/x3/", "/x1/").Replace("/x4/", "/x1/");
			pathsToTry.Add(fallbackPath);
		}
		
		foreach (var basePath in pathsToTry)
		{
			// Try with different extensions
			string[] extensions = new string[] { "", ".png", ".jpg" };
			
			foreach (var ext in extensions)
			{
				string currentPath = basePath;
				
				// Only add extension if not already present
				if (ext != "" && !currentPath.EndsWith(".png") && !currentPath.EndsWith(".jpg"))
				{
					currentPath = basePath + ext;
				}
				else if (ext != "")
				{
					continue; // Skip if path already has extension
				}
				
				// Check if resource exists before trying to load (to avoid error spam)
				if (!ResourceLoader.Exists(currentPath))
				{
					continue;
				}
				
				// Try to load as Texture2D first (imported resource)
				var texture2D = ResourceLoader.Load<Texture2D>(currentPath);
				if (texture2D != null)
				{
					image.texture = texture2D as ImageTexture;
					if (image.texture == null)
					{
						// It's a CompressedTexture2D or similar, get the image from it
						image.rawImage = texture2D.GetImage();
						image.texture = ImageTexture.CreateFromImage(image.rawImage);
					}
					else
					{
						image.rawImage = image.texture.GetImage();
					}
					image.w = (int)texture2D.GetWidth();
					image.h = (int)texture2D.GetHeight();
					setTextureQuality(image);
					return image;
				}
			}
			
			// Try loading as raw file
			foreach (var ext in extensions)
			{
				string currentPath = basePath;
				if (ext != "" && !currentPath.EndsWith(".png") && !currentPath.EndsWith(".jpg"))
				{
					currentPath = basePath + ext;
				}
				else if (ext != "")
				{
					continue;
				}
				
				if (FileAccess.FileExists(currentPath))
				{
					using var file = FileAccess.Open(currentPath, FileAccess.ModeFlags.Read);
					if (file != null)
					{
						byte[] data = file.GetBuffer((long)file.GetLength());
						image.rawImage = new Godot.Image();
						Error err;
						if (currentPath.EndsWith(".png"))
						{
							err = image.rawImage.LoadPngFromBuffer(data);
						}
						else
						{
							err = image.rawImage.LoadJpgFromBuffer(data);
						}
						
						if (err == Error.Ok)
						{
							image.texture = ImageTexture.CreateFromImage(image.rawImage);
							image.w = image.rawImage.GetWidth();
							image.h = image.rawImage.GetHeight();
							setTextureQuality(image);
							return image;
						}
					}
				}
			}
		}
		
		// Return an empty 1x1 image instead of throwing exception
		Cout.LogWarning("Could not load image: " + filename + ", returning empty image");
		image.rawImage = Godot.Image.CreateEmpty(1, 1, false, Godot.Image.Format.Rgba8);
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		image.w = 1;
		image.h = 1;
		return image;
	}

	private static Image __createImage(byte[] imageData)
	{
		if (imageData == null || imageData.Length == 0)
		{
			Cout.LogError("Create Image from byte array fail");
			return null;
		}
		Image image = new Image();
		try
		{
			image.rawImage = new Godot.Image();
			// Try PNG first, then JPG
			Error err = image.rawImage.LoadPngFromBuffer(imageData);
			if (err != Error.Ok)
			{
				err = image.rawImage.LoadJpgFromBuffer(imageData);
			}
			
			if (err == Error.Ok)
			{
				image.texture = ImageTexture.CreateFromImage(image.rawImage);
				image.w = image.rawImage.GetWidth();
				image.h = image.rawImage.GetHeight();
				setTextureQuality(image);
			}
			else
			{
				Cout.LogError("Failed to load image from buffer");
			}
		}
		catch (Exception)
		{
			Cout.LogError("CREAT IMAGE FROM ARRAY FAIL \n" + System.Environment.StackTrace);
		}
		return image;
	}

	private static Image __createImage(Image src, int x, int y, int w, int h, int transform)
	{
		Image image = new Image();
		image.rawImage = Godot.Image.CreateEmpty(w, h, false, Godot.Image.Format.Rgba8);
		
		// Copy region from source
		for (int i = 0; i < w; i++)
		{
			for (int j = 0; j < h; j++)
			{
				int srcX = i;
				if (transform == 2) // Mirror
				{
					srcX = w - 1 - i;
				}
				int srcY = j;
				Color pixel = src.rawImage.GetPixel(x + srcX, y + srcY);
				image.rawImage.SetPixel(i, j, pixel);
			}
		}
		
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		image.w = w;
		image.h = h;
		setTextureQuality(image);
		return image;
	}

	private static Image __createEmptyImage()
	{
		return new Image();
	}

	public static Image __createImage(int w, int h)
	{
		Image image = new Image();
		image.rawImage = Godot.Image.CreateEmpty(w, h, false, Godot.Image.Format.Rgba8);
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		image.w = w;
		image.h = h;
		setTextureQuality(image);
		return image;
	}

	public static int getImageWidth(Image image)
	{
		return image.getWidth();
	}

	public static int getImageHeight(Image image)
	{
		return image.getHeight();
	}

	public int getWidth()
	{
		return w / mGraphics.zoomLevel;
	}

	public int getHeight()
	{
		return h / mGraphics.zoomLevel;
	}

	private static void setTextureQuality(Image img)
	{
		// Godot doesn't have per-texture filter settings in the same way
		// Filter mode is set during drawing
	}

	public static void setTextureQuality(ImageTexture texture)
	{
		// Godot handles texture filtering differently
	}

	public Color[] getColor()
	{
		if (rawImage == null) return new Color[0];
		
		Color[] colors = new Color[w * h];
		for (int y = 0; y < h; y++)
		{
			for (int x = 0; x < w; x++)
			{
				colors[y * w + x] = rawImage.GetPixel(x, y);
			}
		}
		return colors;
	}

	public int getRealImageWidth()
	{
		return w;
	}

	public int getRealImageHeight()
	{
		return h;
	}

	public void getRGB(ref int[] data, int x1, int x2, int x, int y, int w, int h)
	{
		if (rawImage == null) return;
		
		for (int j = 0; j < h; j++)
		{
			for (int i = 0; i < w; i++)
			{
				Color pixel = rawImage.GetPixel(x + i, y + j);
				data[j * w + i] = mGraphics.getIntByColor(pixel);
			}
		}
	}
}
