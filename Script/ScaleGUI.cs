using System.Collections.Generic;
using Godot;

public class ScaleGUI
{
	public static bool scaleScreen;

	public static float WIDTH;

	public static float HEIGHT;

	public static void initScaleGUI()
	{
		Vector2I windowSize = DisplayServer.WindowGetSize();
		Cout.println("Init Scale GUI: Screen.w=" + windowSize.X + " Screen.h=" + windowSize.Y);
		WIDTH = windowSize.X;
		HEIGHT = windowSize.Y;
		scaleScreen = false;
		if (windowSize.X <= 1200)
		{
		}
	}
}
