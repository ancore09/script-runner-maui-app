using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using TestMauiBackend.Models;

namespace TestMauiBackend.Helpers;

public class PowerShellHelper
{
    // method to run a powershell script with a given path and arguments
    public static RunResponse RunScript(string scriptPath, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "powershell.exe";
        startInfo.Arguments = $"-File \"{scriptPath}\" {arguments}";
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;

        Process process = new Process() {StartInfo = startInfo, EnableRaisingEvents = true};
        
        process.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
        
        process.Start();

        process.WaitForExit();
        
        var standardOutput = process.StandardOutput.ReadToEnd();
        
        var errorOutput = process.StandardError.ReadToEnd();
        
        process.Close();
        
        return new RunResponse() {StandardOutput = standardOutput, StandardError = errorOutput};
    }
    
    public static RunResponse Shutdown()
    {
        Task shutdownTask = new Task(async () =>
        {
            await Task.Delay(1000);
            Console.WriteLine("Shutdown");
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "shutdown";
            startInfo.Arguments = "/s /t 5";
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            process.StartInfo = startInfo;
            process.Start();
        });
        shutdownTask.Start();
        return new RunResponse() {StandardOutput = "Shutdown initiated", StandardError = ""};
    }
}