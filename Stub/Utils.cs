using System.Diagnostics;

public class Utils
{
    public static void RunCommand(string command)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            try
            {
                process.PriorityClass = ProcessPriorityClass.RealTime;
                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.Close();
                process.WaitForExit();
            }
            catch
            {

            }
        }
    }

    public static void RunCommand(string command, string arguments)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.Verb = "runas";
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            try
            {
                process.PriorityClass = ProcessPriorityClass.RealTime;
                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.Close();
                process.WaitForExit();
            }
            catch
            {

            }
        }
    }

    public static string RunCommandReturn(string command)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;

            try
            {
                process.PriorityClass = ProcessPriorityClass.RealTime;
                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.Close();
                process.WaitForExit();

                return process.StandardOutput.ReadToEnd();
            }
            catch
            {

            }
        }

        return "";
    }

    public static void RunPowerShell(string command)
    {
        try
        {
            var process = new Process();
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.CreateNoWindow = true;
            processStartInfo.Arguments = command;
            processStartInfo.Verb = "runas";
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.FileName = "powershell";
            process.StartInfo = processStartInfo;
            process.PriorityClass = ProcessPriorityClass.RealTime;
            process.Start();
            process.WaitForExit();
        }
        catch
        {

        }
    }
}