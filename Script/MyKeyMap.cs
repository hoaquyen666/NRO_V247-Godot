using System.Collections;
using Godot;

public class MyKeyMap
{
	private static Hashtable h;
	private static Hashtable hGodot;

	static MyKeyMap()
	{
		h = new Hashtable();
		h.Add(65, 97);  // A
		h.Add(66, 98);  // B
		h.Add(67, 99);  // C
		h.Add(68, 100); // D
		h.Add(69, 101); // E
		h.Add(70, 102); // F
		h.Add(71, 103); // G
		h.Add(72, 104); // H
		h.Add(73, 105); // I
		h.Add(74, 106); // J
		h.Add(75, 107); // K
		h.Add(76, 108); // L
		h.Add(77, 109); // M
		h.Add(78, 110); // N
		h.Add(79, 111); // O
		h.Add(80, 112); // P
		h.Add(81, 113); // Q
		h.Add(82, 114); // R
		h.Add(83, 115); // S
		h.Add(84, 116); // T
		h.Add(85, 117); // U
		h.Add(86, 118); // V
		h.Add(87, 119); // W
		h.Add(88, 120); // X
		h.Add(89, 121); // Y
		h.Add(90, 122); // Z
		
		// Godot key mapping
		hGodot = new Hashtable();
		hGodot.Add(Godot.Key.A, 97);
		hGodot.Add(Godot.Key.B, 98);
		hGodot.Add(Godot.Key.C, 99);
		hGodot.Add(Godot.Key.D, 100);
		hGodot.Add(Godot.Key.E, 101);
		hGodot.Add(Godot.Key.F, 102);
		hGodot.Add(Godot.Key.G, 103);
		hGodot.Add(Godot.Key.H, 104);
		hGodot.Add(Godot.Key.I, 105);
		hGodot.Add(Godot.Key.J, 106);
		hGodot.Add(Godot.Key.K, 107);
		hGodot.Add(Godot.Key.L, 108);
		hGodot.Add(Godot.Key.M, 109);
		hGodot.Add(Godot.Key.N, 110);
		hGodot.Add(Godot.Key.O, 111);
		hGodot.Add(Godot.Key.P, 112);
		hGodot.Add(Godot.Key.Q, 113);
		hGodot.Add(Godot.Key.R, 114);
		hGodot.Add(Godot.Key.S, 115);
		hGodot.Add(Godot.Key.T, 116);
		hGodot.Add(Godot.Key.U, 117);
		hGodot.Add(Godot.Key.V, 118);
		hGodot.Add(Godot.Key.W, 119);
		hGodot.Add(Godot.Key.X, 120);
		hGodot.Add(Godot.Key.Y, 121);
		hGodot.Add(Godot.Key.Z, 122);
		hGodot.Add(Godot.Key.Key0, 48);
		hGodot.Add(Godot.Key.Key1, 49);
		hGodot.Add(Godot.Key.Key2, 50);
		hGodot.Add(Godot.Key.Key3, 51);
		hGodot.Add(Godot.Key.Key4, 52);
		hGodot.Add(Godot.Key.Key5, 53);
		hGodot.Add(Godot.Key.Key6, 54);
		hGodot.Add(Godot.Key.Key7, 55);
		hGodot.Add(Godot.Key.Key8, 56);
		hGodot.Add(Godot.Key.Key9, 57);
		hGodot.Add(Godot.Key.Space, 32);
		hGodot.Add(Godot.Key.F1, -21);
		hGodot.Add(Godot.Key.F2, -22);
		hGodot.Add(Godot.Key.Equal, -25);
		hGodot.Add(Godot.Key.Minus, 45);
		hGodot.Add(Godot.Key.F3, -23);
		hGodot.Add(Godot.Key.Up, -1);
		hGodot.Add(Godot.Key.Down, -2);
		hGodot.Add(Godot.Key.Left, -3);
		hGodot.Add(Godot.Key.Right, -4);
		hGodot.Add(Godot.Key.Backspace, -8);
		hGodot.Add(Godot.Key.Enter, -5);
		hGodot.Add(Godot.Key.Period, 46);
		// Key.At doesn't exist in Godot 4.5, removed
		hGodot.Add(Godot.Key.Tab, -26);
	}

	public static int map(int k)
	{
		object obj = h[k];
		if (obj == null)
		{
			return 0;
		}
		return (int)obj;
	}
	
	public static int mapGodot(Godot.Key k)
	{
		object obj = hGodot[k];
		if (obj == null)
		{
			return 0;
		}
		return (int)obj;
	}
}
