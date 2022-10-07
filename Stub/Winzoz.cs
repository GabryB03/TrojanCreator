using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Taskbar
{
    [DllImport("user32.dll")]
    private static extern int FindWindow(string className, string windowText);

    [DllImport("user32.dll")]
    private static extern int ShowWindow(int hwnd, int command);

    [DllImport("user32.dll")]
    public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

    [DllImport("user32.dll")]
    private static extern int GetDesktopWindow();

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 1;

    protected static int Handle
    {
        get
        {
            return FindWindow("Shell_TrayWnd", "");
        }
    }

    protected static int HandleOfStartButton
    {
        get
        {
            int handleOfDesktop = GetDesktopWindow();
            int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
            return handleOfStartButton;
        }
    }

    private Taskbar()
    {

    }

    public static void Show()
    {
        ShowWindow(Handle, SW_SHOW);
        ShowWindow(HandleOfStartButton, SW_SHOW);
    }

    public static void Hide()
    {
        ShowWindow(Handle, SW_HIDE);
        ShowWindow(HandleOfStartButton, SW_HIDE);
    }
}

public class Desktop
{
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void Show()
    {
        IntPtr hWnd = FindWindow("Progman", "Program Manager");
        ShowWindow(hWnd, 5);
    }

    public static void Hide()
    {
        IntPtr hWnd = FindWindow("Progman", "Program Manager");
        ShowWindow(hWnd, 0);
    }
}

public class DesktopIcons
{
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
    enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    private const int WM_COMMAND = 0x111;

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        private int _Left;
        private int _Top;
        private int _Right;
        private int _Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WINDOWINFO(Boolean? filler) : this()
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }
    }

    private static void ToggleDesktopIcons()
    {
        var toggleDesktopCommand = new IntPtr(0x7402);
        IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
        SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
    }

    private static bool IsVisible()
    {
        IntPtr hWnd = GetWindow(GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD), GetWindow_Cmd.GW_CHILD);
        WINDOWINFO info = new WINDOWINFO();
        info.cbSize = (uint)Marshal.SizeOf(info);
        GetWindowInfo(hWnd, ref info);
        return (info.dwStyle & 0x10000000) == 0x10000000;
    }

    public static void Show()
    {
        if (!IsVisible())
        {
            ToggleDesktopIcons();
        }
    }

    public static void Hide()
    {
        if (IsVisible())
        {
            ToggleDesktopIcons();
        }
    }
}

public class Monitor
{
    private static int WM_SYSCOMMAND = 0x112;
    private static int SC_MONITORPOWER = 0xF170;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    public static void Disable()
    {
        Form f = new Form();
        bool turnOff = true;
        SendMessage(f.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)(turnOff ? 2 : -1));
    }

    [DllImport("user32.dll")]
    static extern void mouse_event(Int32 dwFlags, Int32 dx, Int32 dy, Int32 dwData, UIntPtr dwExtraInfo);

    private const int MOUSEEVENTF_MOVE = 0x0001;

    public static void Enable()
    {
        mouse_event(MOUSEEVENTF_MOVE, 0, 1, 0, UIntPtr.Zero);
        System.Threading.Thread.Sleep(40);
        mouse_event(MOUSEEVENTF_MOVE, 0, -1, 0, UIntPtr.Zero);
    }
}