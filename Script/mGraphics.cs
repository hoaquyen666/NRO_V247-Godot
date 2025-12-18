using System;
using System.Collections;
using Assets.src.e;
using Godot;

public class mGraphics
{
	public static int HCENTER = 1;

	public static int VCENTER = 2;

	public static int LEFT = 4;

	public static int RIGHT = 8;

	public static int TOP = 16;

	public static int BOTTOM = 32;

	private float r;

	private float g;

	private float b;

	private float a;

	public int clipX;

	public int clipY;

	public int clipW;

	public int clipH;

	private bool isClip;

	private bool isTranslate = true;

	private int translateX;

	private int translateY;

	private float translateXf;

	private float translateYf;

	public static int zoomLevel = 1;

	public const int BASELINE = 64;

	public const int SOLID = 0;

	public const int DOTTED = 1;

	public const int TRANS_MIRROR = 2;

	public const int TRANS_MIRROR_ROT180 = 1;

	public const int TRANS_MIRROR_ROT270 = 4;

	public const int TRANS_MIRROR_ROT90 = 7;

	public const int TRANS_NONE = 0;

	public const int TRANS_ROT180 = 3;

	public const int TRANS_ROT270 = 6;

	public const int TRANS_ROT90 = 5;

	public static Hashtable cachedTextures = new Hashtable();

	public static int addYWhenOpenKeyBoard;

	private int clipTX;

	private int clipTY;

	private int currentBGColor;

	private Vector2 pos = new Vector2(0f, 0f);

	private Rect2 rect;

	private Transform2D matrixBackup;

	private Vector2 pivot;

	public Vector2 size = new Vector2(128f, 128f);

	public Vector2 relativePosition = new Vector2(0f, 0f);

	public Color clTrans;

	public static Color transParentColor = new Color(1f, 1f, 1f, 0f);

	// Reference to the Node2D for drawing
	private Node2D drawContext;
	private Color currentColor = new Color(1, 1, 1, 1);

	public void SetDrawContext(Node2D context)
	{
		drawContext = context;
	}

	public Node2D GetDrawContext()
	{
		return drawContext;
	}

	private void cache(string key, Texture2D value)
	{
		if (cachedTextures.Count > 400)
		{
			cachedTextures.Clear();
		}
		if (value.GetWidth() * value.GetHeight() < GameCanvas.w * GameCanvas.h)
		{
			cachedTextures.Add(key, value);
		}
	}

	public void translate(int tx, int ty)
	{
		tx *= zoomLevel;
		ty *= zoomLevel;
		translateX += tx;
		translateY += ty;
		isTranslate = true;
		if (translateX == 0 && translateY == 0)
		{
			isTranslate = false;
		}
	}

	public void translate(float x, float y)
	{
		translateXf += x;
		translateYf += y;
		isTranslate = true;
		if (translateXf == 0f && translateYf == 0f)
		{
			isTranslate = false;
		}
	}

	public int getTranslateX()
	{
		return translateX / zoomLevel;
	}

	public int getTranslateY()
	{
		return translateY / zoomLevel + addYWhenOpenKeyBoard;
	}

	public void setClip(int x, int y, int w, int h)
	{
		x *= zoomLevel;
		y *= zoomLevel;
		w *= zoomLevel;
		h *= zoomLevel;
		clipTX = translateX;
		clipTY = translateY;
		clipX = x;
		clipY = y;
		clipW = w;
		clipH = h;
		isClip = true;
	}

	public int getClipX()
	{
		return GameScr.cmx;
	}

	public int getClipY()
	{
		return GameScr.cmy;
	}

	public int getClipWidth()
	{
		return GameScr.gW;
	}

	public int getClipHeight()
	{
		return GameScr.gH;
	}

	public void fillRect(int x, int y, int w, int h, int color, int alpha)
	{
		float alphaF = (float)alpha / 255f;
		setColor(color, alphaF);
		fillRect(x, y, w, h);
	}

	public void drawLine(int x1, int y1, int x2, int y2)
	{
		if (drawContext == null) return;
		
		x1 *= zoomLevel;
		y1 *= zoomLevel;
		x2 *= zoomLevel;
		y2 *= zoomLevel;
		
		if (isTranslate)
		{
			x1 += translateX;
			y1 += translateY;
			x2 += translateX;
			y2 += translateY;
		}
		
		Color lineColor = new Color(r, g, b, a);
		drawContext.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), lineColor, zoomLevel);
	}

	public Color setColorMiniMap(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		float num4 = (float)num / 256f;
		float num5 = (float)num2 / 256f;
		float num6 = (float)num3 / 256f;
		return new Color(num6, num5, num4);
	}

	public float[] getRGB(Color cl)
	{
		float num = 256f * cl.R;
		float num2 = 256f * cl.G;
		float num3 = 256f * cl.B;
		return new float[3] { num, num2, num3 };
	}

	public void drawRect(int x, int y, int w, int h)
	{
		int num = 1;
		fillRect(x, y, w, num);
		fillRect(x, y, num, h);
		fillRect(x + w, y, num, h + 1);
		fillRect(x, y + h, w + 1, num);
	}

	public void fillRect(int x, int y, int w, int h)
	{
		if (drawContext == null) return;
		
		x *= zoomLevel;
		y *= zoomLevel;
		w *= zoomLevel;
		h *= zoomLevel;
		if (w < 0 || h < 0)
		{
			return;
		}
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		
		Color fillColor = new Color(r, g, b, a);
		Rect2 rectToDraw = new Rect2(x, y, w, h);
		
		// Handle clipping
		if (isClip)
		{
			int clipXAdjusted = clipX;
			int clipYAdjusted = clipY;
			if (isTranslate)
			{
				clipXAdjusted += clipTX;
				clipYAdjusted += clipTY;
			}
			Rect2 clipRect = new Rect2(clipXAdjusted, clipYAdjusted, clipW, clipH);
			rectToDraw = rectToDraw.Intersection(clipRect);
			if (rectToDraw.Size.X <= 0 || rectToDraw.Size.Y <= 0)
			{
				return;
			}
		}
		
		drawContext.DrawRect(rectToDraw, fillColor);
	}

	public void setColor(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		b = (float)num / 256f;
		g = (float)num2 / 256f;
		r = (float)num3 / 256f;
		a = 1f;
		currentColor = new Color(r, g, b, a);
	}

	public void setColor(Color color)
	{
		b = color.B;
		g = color.G;
		r = color.R;
		a = color.A;
		currentColor = color;
	}

	public void setBgColor(int rgb)
	{
		if (rgb != currentBGColor)
		{
			currentBGColor = rgb;
			int num = rgb & 0xFF;
			int num2 = (rgb >> 8) & 0xFF;
			int num3 = (rgb >> 16) & 0xFF;
			b = (float)num / 256f;
			g = (float)num2 / 256f;
			r = (float)num3 / 256f;
			// In Godot, background color would be set differently
			// RenderingServer.SetDefaultClearColor(new Color(r, g, b));
		}
	}

	public void drawString(string s, int x, int y, Font font)
	{
		if (drawContext == null) return;
		
		x *= zoomLevel;
		y *= zoomLevel;
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		
		// Draw text using Godot's DrawString
		drawContext.DrawString(font, new Vector2(x, y), s, HorizontalAlignment.Left, -1, 16, currentColor);
	}

	public void drawStringWithColor(string s, int x, int y, Godot.Color color, Godot.Font font, int align)
	{
		if (drawContext == null) return;
		
		x *= zoomLevel;
		y *= zoomLevel;
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		
		HorizontalAlignment hAlign = HorizontalAlignment.Left;
		if (align == 1) hAlign = HorizontalAlignment.Right;
		else if (align == 2) hAlign = HorizontalAlignment.Center;
		
		if (font != null)
		{
			drawContext.DrawString(font, new Vector2(x, y + 16), s, hAlign, -1, 16, color);
		}
	}

	public void setColor(int rgb, float alpha)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		b = (float)num / 256f;
		g = (float)num2 / 256f;
		r = (float)num3 / 256f;
		a = alpha;
		currentColor = new Color(r, g, b, a);
	}

	private void UpdatePos(int anchor)
	{
		Vector2 vector = new Vector2(0f, 0f);
		int screenWidth = Main.GetScreenWidth();
		int screenHeight = Main.GetScreenHeight();
		
		switch (anchor)
		{
		case 3:
			vector = new Vector2(size.X / 2f, size.Y / 2f);
			break;
		case 20:
			vector = new Vector2(0f, 0f);
			break;
		case 17:
			vector = new Vector2(screenWidth / 2, 0f);
			break;
		case 24:
			vector = new Vector2(screenWidth, 0f);
			break;
		case 6:
			vector = new Vector2(0f, screenHeight / 2);
			break;
		case 10:
			vector = new Vector2(screenWidth, screenHeight / 2);
			break;
		case 36:
			vector = new Vector2(0f, screenHeight);
			break;
		case 33:
			vector = new Vector2(screenWidth / 2, screenHeight);
			break;
		case 40:
			vector = new Vector2(screenWidth, screenHeight);
			break;
		}
		pos = vector + relativePosition;
		rect = new Rect2(pos.X - size.X * 0.5f, pos.Y - size.Y * 0.5f, size.X, size.Y);
		pivot = new Vector2(rect.Position.X + rect.Size.X * 0.5f, rect.Position.Y + rect.Size.Y * 0.5f);
	}

	public void drawRegion(Image arg0, int x0, int y0, int w0, int h0, int arg5, int x, int y, int arg8)
	{
		if (arg0 != null)
		{
			x *= zoomLevel;
			y *= zoomLevel;
			x0 *= zoomLevel;
			y0 *= zoomLevel;
			w0 *= zoomLevel;
			h0 *= zoomLevel;
			_drawRegion(arg0, x0, y0, w0, h0, arg5, x, y, arg8);
		}
	}

	public void drawRegion(Image arg0, int x0, int y0, int w0, int h0, int arg5, float x, float y, int arg8)
	{
		if (arg0 != null)
		{
			x *= (float)zoomLevel;
			y *= (float)zoomLevel;
			x0 *= zoomLevel;
			y0 *= zoomLevel;
			w0 *= zoomLevel;
			h0 *= zoomLevel;
			__drawRegion(arg0, x0, y0, w0, h0, arg5, x, y, arg8);
		}
	}

	public void drawRegion(Image arg0, int x0, int y0, int w0, int h0, int arg5, int x, int y, int arg8, bool isClip)
	{
		drawRegion(arg0, x0, y0, w0, h0, arg5, x, y, arg8);
	}

	public void __drawRegion(Image image, int x0, int y0, int w, int h, int transform, float x, float y, int anchor)
	{
		if (image == null || image.texture == null || drawContext == null)
		{
			return;
		}
		if (isTranslate)
		{
			x += (float)translateX;
			y += (float)translateY;
		}
		float num = w;
		float num2 = h;
		float num5 = 0f;
		float num6 = 0f;
		
		if ((anchor & HCENTER) == HCENTER)
		{
			num5 -= num / 2f;
		}
		if ((anchor & VCENTER) == VCENTER)
		{
			num6 -= num2 / 2f;
		}
		if ((anchor & RIGHT) == RIGHT)
		{
			num5 -= num;
		}
		if ((anchor & BOTTOM) == BOTTOM)
		{
			num6 -= num2;
		}
		x += num5;
		y += num6;
		
		// Handle clipping
		Rect2 srcRect = new Rect2(x0, y0, w, h);
		Rect2 destRect = new Rect2(x, y, w, h);
		
		if (isClip)
		{
			int clipXAdjusted = clipX;
			int clipYAdjusted = clipY;
			if (isTranslate)
			{
				clipXAdjusted += clipTX;
				clipYAdjusted += clipTY;
			}
			Rect2 clipRect = new Rect2(clipXAdjusted, clipYAdjusted, clipW, clipH);
			Rect2 visibleRect = destRect.Intersection(clipRect);
			if (visibleRect.Size.X <= 0 || visibleRect.Size.Y <= 0)
			{
				return;
			}
			// Adjust source rect based on clipping
			float offsetX = visibleRect.Position.X - destRect.Position.X;
			float offsetY = visibleRect.Position.Y - destRect.Position.Y;
			srcRect = new Rect2(x0 + offsetX, y0 + offsetY, visibleRect.Size.X, visibleRect.Size.Y);
			destRect = visibleRect;
		}
		
		// Handle transforms
		bool flipH = false;
		bool flipV = false;
		float rotation = 0f;
		
		switch (transform)
		{
		case TRANS_MIRROR: // 2
			flipH = true;
			break;
		case TRANS_MIRROR_ROT180: // 1
			flipV = true;
			break;
		case TRANS_ROT180: // 3
			flipH = true;
			flipV = true;
			break;
		case TRANS_ROT90: // 5
			rotation = Mathf.Pi / 2f;
			break;
		case TRANS_ROT270: // 6
			rotation = -Mathf.Pi / 2f;
			break;
		case TRANS_MIRROR_ROT270: // 4
			rotation = -Mathf.Pi / 2f;
			flipH = true;
			break;
		case TRANS_MIRROR_ROT90: // 7
			rotation = -Mathf.Pi / 2f;
			flipV = true;
			break;
		}
		
		// Draw the texture region
		if (rotation != 0f)
		{
			// For rotated textures, we need to handle transform manually
			Vector2 center = destRect.Position + destRect.Size / 2;
			// For now, just draw without rotation for complex cases
			drawContext.DrawTextureRectRegion(image.texture, destRect, srcRect, new Color(1, 1, 1, 1), false, flipH);
		}
		else
		{
			drawContext.DrawTextureRectRegion(image.texture, destRect, srcRect, new Color(1, 1, 1, 1), false, flipH);
		}
	}

	public void _drawRegion(Image image, float x0, float y0, int w, int h, int transform, int x, int y, int anchor)
	{
		__drawRegion(image, (int)x0, (int)y0, w, h, transform, (float)x, (float)y, anchor);
	}

	public void drawRegionGui(Image image, float x0, float y0, int w, int h, int transform, float x, float y, int anchor)
	{
		x *= (float)zoomLevel;
		y *= (float)zoomLevel;
		x0 *= (float)zoomLevel;
		y0 *= (float)zoomLevel;
		w *= zoomLevel;
		h *= zoomLevel;
	}

	public void drawRegion2(Image image, float x0, float y0, int w, int h, int transform, int x, int y, int anchor)
	{
		if (image == null || drawContext == null) return;
		
		if (isTranslate)
		{
			x += translateX;
			y += translateY;
		}
		
		float num5 = w;
		float num6 = h;
		float num7 = 0f;
		float num8 = 0f;
		if ((anchor & HCENTER) == HCENTER)
		{
			num7 -= num5 / 2f;
		}
		if ((anchor & VCENTER) == VCENTER)
		{
			num8 -= num6 / 2f;
		}
		if ((anchor & RIGHT) == RIGHT)
		{
			num7 -= num5;
		}
		if ((anchor & BOTTOM) == BOTTOM)
		{
			num8 -= num6;
		}
		x += (int)num7;
		y += (int)num8;
		
		Rect2 srcRect = new Rect2(x0, y0, w, h);
		Rect2 destRect = new Rect2(x, y, w, h);
		
		if (isClip)
		{
			int clipXAdjusted = clipX;
			int clipYAdjusted = clipY;
			if (isTranslate)
			{
				clipXAdjusted += clipTX;
				clipYAdjusted += clipTY;
			}
			Rect2 clipRect = new Rect2(clipXAdjusted, clipYAdjusted, clipW, clipH);
			Rect2 visibleRect = destRect.Intersection(clipRect);
			if (visibleRect.Size.X <= 0 || visibleRect.Size.Y <= 0)
			{
				return;
			}
			destRect = visibleRect;
		}
		
		drawContext.DrawTextureRectRegion(image.texture, destRect, srcRect, image.colorBlend);
	}

	public void drawImagaByDrawTexture(Image image, float x, float y)
	{
		if (drawContext == null || image == null) return;
		
		x *= (float)zoomLevel;
		y *= (float)zoomLevel;
		Rect2 destRect = new Rect2(x + translateX, y + translateY, image.getRealImageWidth(), image.getRealImageHeight());
		drawContext.DrawTextureRect(image.texture, destRect, false);
	}

	public void drawImage(Image image, int x, int y, int anchor)
	{
		if (image != null)
		{
			drawRegion(image, 0, 0, getImageWidth(image), getImageHeight(image), 0, x, y, anchor);
		}
	}

	public void drawImageFog(Image image, int x, int y, int anchor)
	{
		if (image != null)
		{
			drawRegion(image, 0, 0, image.w, image.h, 0, x, y, anchor);
		}
	}

	public void drawImage(Image image, int x, int y)
	{
		if (image != null)
		{
			drawRegion(image, 0, 0, getImageWidth(image), getImageHeight(image), 0, x, y, TOP | LEFT);
		}
	}

	public void drawImage(Image image, float x, float y, int anchor)
	{
		if (image != null)
		{
			drawRegion(image, 0, 0, getImageWidth(image), getImageHeight(image), 0, x, y, anchor);
		}
	}

	public void drawRoundRect(int x, int y, int w, int h, int arcWidth, int arcHeight)
	{
		drawRect(x, y, w, h);
	}

	public void fillRoundRect(int x, int y, int width, int height, int arcWidth, int arcHeight)
	{
		fillRect(x, y, width, height);
	}

	public void reset()
	{
		isClip = false;
		isTranslate = false;
		translateX = 0;
		translateY = 0;
	}

	public Rect2 intersectRect(Rect2 r1, Rect2 r2)
	{
		return r1.Intersection(r2);
	}

	public void drawImageScale(Image image, int x, int y, int w, int h, int tranform)
	{
		if (drawContext == null || image == null) return;
		
		x *= zoomLevel;
		y *= zoomLevel;
		w *= zoomLevel;
		h *= zoomLevel;
		
		bool flipH = tranform != 0;
		Rect2 destRect = new Rect2(x + translateX, y + translateY, flipH ? -w : w, h);
		drawContext.DrawTextureRect(image.texture, destRect, false);
	}

	public void drawImageSimple(Image image, int x, int y)
	{
		if (drawContext == null || image == null) return;
		
		x *= zoomLevel;
		y *= zoomLevel;
		Rect2 destRect = new Rect2(x, y, image.w, image.h);
		drawContext.DrawTextureRect(image.texture, destRect, false);
	}

	public static int getImageWidth(Image image)
	{
		return image.getWidth();
	}

	public static int getImageHeight(Image image)
	{
		return image.getHeight();
	}

	public static bool isNotTranColor(Color color)
	{
		if (color.A == 0 || color == transParentColor)
		{
			return false;
		}
		return true;
	}

	public static Image blend(Image img0, float level, int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		float num4 = (float)num / 256f;
		float num5 = (float)num2 / 256f;
		float num6 = (float)num3 / 256f;
		Color color = new Color(num6, num5, num4);
		
		Image image = Image.createImage(img0.getRealImageWidth(), img0.getRealImageHeight());
		
		for (int y = 0; y < img0.h; y++)
		{
			for (int x = 0; x < img0.w; x++)
			{
				Color pixel = img0.rawImage.GetPixel(x, y);
				if (isNotTranColor(pixel))
				{
					float newR = (color.R - pixel.R) * level + pixel.R;
					float newG = (color.G - pixel.G) * level + pixel.G;
					float newB = (color.B - pixel.B) * level + pixel.B;
					newR = Mathf.Clamp(newR, 0f, 1f);
					newG = Mathf.Clamp(newG, 0f, 1f);
					newB = Mathf.Clamp(newB, 0f, 1f);
					image.rawImage.SetPixel(x, y, new Color(newR, newG, newB, pixel.A));
				}
				else
				{
					image.rawImage.SetPixel(x, y, pixel);
				}
			}
		}
		
		image.texture = ImageTexture.CreateFromImage(image.rawImage);
		Cout.LogError2("BLEND ----------------------------------------------------");
		return image;
	}

	public static Color setColorObj(int rgb)
	{
		int num = rgb & 0xFF;
		int num2 = (rgb >> 8) & 0xFF;
		int num3 = (rgb >> 16) & 0xFF;
		float num4 = (float)num / 256f;
		float num5 = (float)num2 / 256f;
		float num6 = (float)num3 / 256f;
		return new Color(num6, num5, num4);
	}

	public void fillTrans(Image imgTrans, int x, int y, int w, int h)
	{
		setColor(0, 0.5f);
		fillRect(x * zoomLevel, y * zoomLevel, w * zoomLevel, h * zoomLevel);
	}

	public static int blendColor(float level, int color, int colorBlend)
	{
		Color color2 = setColorObj(colorBlend);
		float num = color2.R * 255f;
		float num2 = color2.G * 255f;
		float num3 = color2.B * 255f;
		Color color3 = setColorObj(color);
		float num4 = (num + color3.R) * level + color3.R;
		float num5 = (num2 + color3.G) * level + color3.G;
		float num6 = (num3 + color3.B) * level + color3.B;
		num4 = Mathf.Clamp(num4, 0f, 255f);
		num5 = Mathf.Clamp(num5, 0f, 255f);
		num6 = Mathf.Clamp(num6, 0f, 255f);
		return ((int)num6 & 0xFF) | (((int)num5 & 0xFF) << 8) | (((int)num4 & 0xFF) << 16);
	}

	public static int getIntByColor(Color cl)
	{
		float num = cl.R * 255f;
		float num2 = cl.B * 255f;
		float num3 = cl.G * 255f;
		return (((int)num & 0xFF) << 16) | (((int)num3 & 0xFF) << 8) | ((int)num2 & 0xFF);
	}

	public static int getRealImageWidth(Image img)
	{
		return img.w;
	}

	public static int getRealImageHeight(Image img)
	{
		return img.h;
	}

	public void fillArg(int i, int j, int k, int l, int m, int n)
	{
		fillRect(i * zoomLevel, j * zoomLevel, k * zoomLevel, l * zoomLevel);
	}

	public void CreateLineMaterial()
	{
		// In Godot, we use built-in drawing functions, no material needed
	}

	public void drawlineGL(MyVector totalLine)
	{
		if (drawContext == null) return;
		
		for (int i = 0; i < totalLine.size(); i++)
		{
			mLine mLine2 = (mLine)totalLine.elementAt(i);
			Color lineColor = new Color(mLine2.r, mLine2.g, mLine2.b, mLine2.a);
			int x1 = mLine2.x1 * zoomLevel;
			int y1 = mLine2.y1 * zoomLevel;
			int x2 = mLine2.x2 * zoomLevel;
			int y2 = mLine2.y2 * zoomLevel;
			if (isTranslate)
			{
				x1 += translateX;
				y1 += translateY;
				x2 += translateX;
				y2 += translateY;
			}
			drawContext.DrawLine(new Vector2(x1, y1), new Vector2(x2, y2), lineColor, zoomLevel);
		}
		totalLine.removeAllElements();
	}

	public void drawLine(mGraphics g, int x, int y, int xTo, int yTo, int nLine, int color)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < nLine; i++)
		{
			myVector.addElement(new mLine(x, y, xTo + i, yTo + i, color));
		}
		g.drawlineGL(myVector);
	}

	internal void drawRegion(Small img, int p1, int p2, int p3, int p4, int transform, int x, int y, int anchor)
	{
		throw new NotImplementedException();
	}
}
