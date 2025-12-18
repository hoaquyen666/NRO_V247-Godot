using System.Net.NetworkInformation;
using System.Threading;
using Godot;

public partial class Main : Node2D
{
	public static Main main;

	public static mGraphics g;

	public static GameMidlet midlet;

	public static string res = "res";

	public static string mainThreadName;

	public static bool started;

	public static bool isIpod;

	public static bool isIphone4;

	public static bool isPC;

	public static bool isWindowsPhone;

	public static bool isIPhone;

	public static bool IphoneVersionApp;

	public static string IMEI;

	public static int versionIp;

	public static int numberQuit = 1;

	public static int typeClient = 4;

	public const sbyte PC_VERSION = 4;

	public const sbyte IP_APPSTORE = 5;

	public const sbyte WINDOWSPHONE = 6;

	private int level;

	public const sbyte IP_JB = 3;

	private int updateCount;

	private int paintCount;

	private int count;

	private int fps;

	private int max;

	private int up;

	private int upmax;

	private long timefps;

	private long timeup;

	private bool isRun;

	public static int waitTick;

	public static int f;

	public static bool isResume;

	public static bool isMiniApp = true;

	public static bool isQuitApp;

	private Vector2 lastMousePos = new Vector2();

	public static int a = 1;

	public static bool isCompactDevice = true;

	// Fixed timestep for game logic (matching Unity's FixedUpdate at ~60fps)
	private const double FixedTimeStep = 1.0 / 60.0;
	private double accumulator = 0.0;

	public override void _Ready()
	{
		Engine.MaxFps = 60;
		if (started)
		{
			return;
		}
		if (Thread.CurrentThread.Name != "Main")
		{
			Thread.CurrentThread.Name = "Main";
		}
		mainThreadName = Thread.CurrentThread.Name;
		isPC = true;
		started = true;
		if (isPC)
		{
			level = Rms.loadRMSInt("levelScreenKN");
			if (level == 1)
			{
				DisplayServer.WindowSetSize(new Vector2I(720, 320));
			}
			else
			{
				DisplayServer.WindowSetSize(new Vector2I(1024, 600));
			}
		}
	}

	private void SetInit()
	{
		SetProcess(true);
	}

	public void setsizeChange()
	{
		if (!isRun)
		{
			DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
			isCompactDevice = detectCompactDevice();
			if (main == null)
			{
				main = this;
			}
			isRun = true;
			ScaleGUI.initScaleGUI();
			if (isPC)
			{
				IMEI = OS.GetUniqueId();
			}
			else
			{
				IMEI = GetMacAddress();
			}
			isPC = true;
			if (isWindowsPhone)
			{
				typeClient = 6;
			}
			if (isPC)
			{
				typeClient = 4;
			}
			if (IphoneVersionApp)
			{
				typeClient = 5;
			}
			if (iPhoneSettings.generation == iPhoneGeneration.iPodTouch4Gen)
			{
				isIpod = true;
			}
			if (iPhoneSettings.generation == iPhoneGeneration.iPhone4)
			{
				isIphone4 = true;
			}
			g = new mGraphics();
			midlet = new GameMidlet();
			TileMap.loadBg();
			Paint.loadbg();
			PopUp.loadBg();
			GameScr.loadBg();
			InfoMe.gI().loadCharId();
			Panel.loadBg();
			Menu.loadBg();
			Key.mapKeyPC();
			SoundMn.gI().loadSound(TileMap.mapID);
			g.CreateLineMaterial();
		}
	}

	public static void setBackupIcloud(string path)
	{
	}

	public string GetMacAddress()
	{
		string empty = string.Empty;
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		for (int i = 0; i < allNetworkInterfaces.Length; i++)
		{
			PhysicalAddress physicalAddress = allNetworkInterfaces[i].GetPhysicalAddress();
			if (physicalAddress.ToString() != string.Empty)
			{
				return physicalAddress.ToString();
			}
		}
		return string.Empty;
	}

	public void doClearRMS()
	{
		if (isPC)
		{
			int num = Rms.loadRMSInt("lastZoomlevel");
			if (num != mGraphics.zoomLevel)
			{
				Rms.clearAll();
				Rms.saveRMSInt("lastZoomlevel", mGraphics.zoomLevel);
				Rms.saveRMSInt("levelScreenKN", level);
			}
		}
	}

	public static void closeKeyBoard()
	{
		// Only call on mobile platforms to avoid warning on PC
		if (!isPC && DisplayServer.HasFeature(DisplayServer.Feature.VirtualKeyboard))
		{
			DisplayServer.VirtualKeyboardHide();
		}
	}

	public override void _Process(double delta)
	{
		// Fixed timestep accumulator for game logic
		accumulator += delta;
		
		while (accumulator >= FixedTimeStep)
		{
			FixedUpdate();
			accumulator -= FixedTimeStep;
		}
		
		// Request redraw for rendering
		QueueRedraw();
	}

	private void FixedUpdate()
	{
		Rms.update();
		count++;
		if (count >= 10)
		{
			if (up == 0)
			{
				timeup = mSystem.currentTimeMillis();
			}
			else if (mSystem.currentTimeMillis() - timeup > 1000)
			{
				upmax = up;
				up = 0;
				timeup = mSystem.currentTimeMillis();
			}
			up++;
			setsizeChange();
			updateCount++;
			ipKeyboard.update();
			GameMidlet.gameCanvas.update();
			Image.update();
			DataInputStream.update();
			f++;
			if (f > 8)
			{
				f = 0;
			}
			if (!isPC)
			{
				int num = 1 / a;
			}
		}
	}

	public override void _Draw()
	{
		if (count >= 10)
		{
			if (fps == 0)
			{
				timefps = mSystem.currentTimeMillis();
			}
			else if (mSystem.currentTimeMillis() - timefps > 1000)
			{
				max = fps;
				fps = 0;
				timefps = mSystem.currentTimeMillis();
			}
			fps++;

			Session_ME.update();
			Session_ME2.update();
			if (paintCount <= updateCount)
			{
				// Set the current graphics context for drawing
				g.SetDrawContext(this);
				GameMidlet.gameCanvas.paint(g);
				paintCount++;
				g.reset();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		checkInput(@event);
	}

	private void checkInput(InputEvent @event)
	{
		// Guard against null gameCanvas during initialization
		if (GameMidlet.gameCanvas == null)
		{
			return;
		}
		
		Vector2 viewportSize = GetViewportRect().Size;
		
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left)
			{
				Vector2 mousePos = mouseButton.Position;
				int x = (int)(mousePos.X / mGraphics.zoomLevel);
				int y = (int)(mousePos.Y / mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard;
				
				if (mouseButton.Pressed)
				{
					GameMidlet.gameCanvas.pointerPressed(x, y);
				}
				else
				{
					GameMidlet.gameCanvas.pointerReleased(x, y);
				}
				lastMousePos.X = mousePos.X / mGraphics.zoomLevel;
				lastMousePos.Y = mousePos.Y / mGraphics.zoomLevel + mGraphics.addYWhenOpenKeyBoard;
			}
			
			// Handle mouse wheel
			if (mouseButton.ButtonIndex == MouseButton.WheelUp)
			{
				GameMidlet.gameCanvas.scrollMouse(1);
			}
			else if (mouseButton.ButtonIndex == MouseButton.WheelDown)
			{
				GameMidlet.gameCanvas.scrollMouse(-1);
			}
		}
		
		if (@event is InputEventMouseMotion mouseMotion)
		{
			Vector2 mousePos = mouseMotion.Position;
			int x = (int)(mousePos.X / mGraphics.zoomLevel);
			int y = (int)(mousePos.Y / mGraphics.zoomLevel) + mGraphics.addYWhenOpenKeyBoard;
			
			if (Input.IsMouseButtonPressed(MouseButton.Left))
			{
				GameMidlet.gameCanvas.pointerDragged(x, y);
			}
			
			if (isPC)
			{
				GameMidlet.gameCanvas.pointerMouse(x, y);
			}
			
			lastMousePos.X = mousePos.X / mGraphics.zoomLevel;
			lastMousePos.Y = mousePos.Y / mGraphics.zoomLevel + mGraphics.addYWhenOpenKeyBoard;
		}
		
		if (@event is InputEventKey keyEvent)
		{
			int keyCode = MyKeyMap.mapGodot(keyEvent.Keycode);
			
			// Handle shift key combinations
			if (Input.IsKeyPressed(Godot.Key.Shift))
			{
				if (keyEvent.Keycode == Godot.Key.Key2)
				{
					keyCode = 64; // @
				}
				else if (keyEvent.Keycode == Godot.Key.Minus)
				{
					keyCode = 95; // _
				}
			}
			
			if (keyCode != 0)
			{
				if (keyEvent.Pressed && !keyEvent.Echo)
				{
					GameMidlet.gameCanvas.keyPressedz(keyCode);
				}
				else if (!keyEvent.Pressed)
				{
					GameMidlet.gameCanvas.keyReleasedz(keyCode);
				}
			}
		}
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest)
		{
			OnApplicationQuit();
		}
		else if (what == NotificationApplicationFocusOut)
		{
			OnApplicationPause(true);
		}
		else if (what == NotificationApplicationFocusIn)
		{
			OnApplicationPause(false);
		}
	}

	private void OnApplicationQuit()
	{
		GD.PrintErr("APP QUIT");
		GameCanvas.bRun = false;
		Session_ME.gI().close();
		Session_ME2.gI().close();
		if (isPC)
		{
			GetTree().Quit();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		isResume = false;
		if (paused)
		{
			if (GameCanvas.isWaiting())
			{
				isQuitApp = true;
			}
		}
		else
		{
			isResume = true;
		}
		// Virtual keyboard handling would go here
		if (isQuitApp)
		{
			GetTree().Quit();
		}
	}

	public static void exit()
	{
		if (isPC)
		{
			main.OnApplicationQuit();
		}
		else
		{
			a = 0;
		}
	}

	public static bool detectCompactDevice()
	{
		if (iPhoneSettings.generation == iPhoneGeneration.iPhone || iPhoneSettings.generation == iPhoneGeneration.iPhone3G || iPhoneSettings.generation == iPhoneGeneration.iPodTouch1Gen || iPhoneSettings.generation == iPhoneGeneration.iPodTouch2Gen)
		{
			return false;
		}
		return true;
	}

	public static bool checkCanSendSMS()
	{
		if (iPhoneSettings.generation == iPhoneGeneration.iPhone3GS || iPhoneSettings.generation == iPhoneGeneration.iPhone4 || iPhoneSettings.generation > iPhoneGeneration.iPodTouch4Gen)
		{
			return true;
		}
		return false;
	}
	
	// Helper to get screen dimensions
	public static int GetScreenWidth()
	{
		return (int)((Main)main).GetViewportRect().Size.X;
	}
	
	public static int GetScreenHeight()
	{
		return (int)((Main)main).GetViewportRect().Size.Y;
	}
}
