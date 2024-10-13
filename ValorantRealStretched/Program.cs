using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
class Program
{
    static bool IsProcessRunning(string processName)
    {
        Process[] processes = Process.GetProcessesByName(processName);
        return processes.Length > 0;
    }
    static void Main()
    {
        string baseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"AppData\Local\VALORANT\Saved\Config");

        int resolutionWidth = 2560;
        int resolutionHeight = 1440;
        int hz = 170;

        string choice;

    start:
        Console.Clear();
        ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        Random random = new Random();
        ConsoleColor randomColor = colors[random.Next(colors.Length)];

        Console.ForegroundColor = randomColor;
        Console.SetWindowSize(150, 20);
        Console.SetBufferSize(150, 20);

        Console.WriteLine(@"
 /$$    /$$          /$$              /$$$$$$   /$$                           /$$               /$$                       /$$
| $$   | $$         | $$             /$$__  $$ | $$                          | $$              | $$                      | $$
| $$   | $$ /$$$$$$ | $$  /$$$$$$   | $$  \__//$$$$$$    /$$$$$$   /$$$$$$  /$$$$$$    /$$$$$$$| $$$$$$$   /$$$$$$   /$$$$$$$
|  $$ / $$/|____  $$| $$ /$$__  $$  |  $$$$$$|_  $$_/   /$$__  $$ /$$__  $$|_  $$_/   /$$_____/| $$__  $$ /$$__  $$ /$$__  $$
 \  $$ $$/  /$$$$$$$| $$| $$  \ $$   \____  $$ | $$    | $$  \__/| $$$$$$$$  | $$    | $$      | $$  \ $$| $$$$$$$$| $$  | $$
  \  $$$/  /$$__  $$| $$| $$  | $$   /$$  \ $$ | $$ /$$| $$      | $$_____/  | $$ /$$| $$      | $$  | $$| $$_____/| $$  | $$
   \  $/  |  $$$$$$$| $$|  $$$$$$/  |  $$$$$$/ |  $$$$/| $$      |  $$$$$$$  |  $$$$/|  $$$$$$$| $$  | $$|  $$$$$$$|  $$$$$$$
    \_/    \_______/|__/ \______/   \______/   \___/  |__/       \_______/   \___/   \_______/|__/  |__/ \_______/ \_______/
        ");

        Console.ResetColor();
        string processName = "VALORANT-Win64-Shipping";
        if (IsProcessRunning(processName)) //VALORANT-Win64-Shipping
        {
            string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "winexp", "winexp.exe");

            Process.Start(appPath);
            Console.WriteLine("Select Valorant and remove *WS_BORDER* on Style and set Window State *Maximized* on Syze & Position");
            while(IsProcessRunning("winexp"))
            {
                
            }

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);

            Process[] processes = Process.GetProcessesByName(processName);

            if (processes.Length > 0)
            {
                foreach (var prc in processes)
                {
                    string name = prc.ProcessName;
                    int pid = prc.Id;
                    try
                    {
                        prc.Kill();
                        prc.WaitForExit();
                        //Console.WriteLine($"Process: {name} (PID: {pid}) Closed");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.ReadLine();
                        return;
                    }
                }
            }
        }

        Console.WriteLine("1) 1440 x 1080");
        Console.WriteLine("2) 1250 x 720");
        Console.WriteLine("3) OFF (2560 x 1440)");
        choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                resolutionWidth = 1440;
                resolutionHeight = 1080;
                break;
            case "2":
                resolutionWidth = 1250;
                resolutionHeight = 720;
                break;
            case "3":
                break;
            default:
                Console.WriteLine("ERROR!");
                goto start;
        }

        Process process = new Process();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "/c " + $"ChangeScreenResolution.exe /w={resolutionWidth} /h={resolutionHeight} /d=0 /f={hz}";
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("Output:");
            Console.WriteLine(output);
        }
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine("Error:");
            Console.WriteLine(error);
        }

        if(choice == "3")
        {
            Console.WriteLine("Done!");
            Console.ReadLine();
            return;
        }

        foreach (var dir in Directory.GetDirectories(baseDir, "*-eu"))
        {
            if (Path.GetFileName(dir).StartsWith("DESKTOP-", StringComparison.OrdinalIgnoreCase)) continue;
            
            string settingsFile = Path.Combine(dir, "Windows", "GameUserSettings.ini");
            if (File.Exists(settingsFile))
            {
                Console.WriteLine($"Edit File: {settingsFile}");

                try
                {
                    string fileContent = File.ReadAllText(settingsFile);

                    fileContent = Regex.Replace(fileContent, @"bShouldLetterbox=.*", "bShouldLetterbox=False");
                    fileContent = Regex.Replace(fileContent, @"ResolutionSizeX=.*", $"ResolutionSizeX={resolutionWidth}");
                    fileContent = Regex.Replace(fileContent, @"ResolutionSizeY=.*", $"ResolutionSizeY={resolutionHeight}");
                    fileContent = Regex.Replace(fileContent, @"LastUserConfirmedResolutionSizeX=.*", $"LastUserConfirmedResolutionSizeX={resolutionWidth}");
                    fileContent = Regex.Replace(fileContent, @"LastUserConfirmedResolutionSizeY=.*", $"LastUserConfirmedResolutionSizeY={resolutionHeight}");
                    fileContent = Regex.Replace(fileContent, @"LastUserConfirmedDesiredScreenWidth=.*", $"LastUserConfirmedDesiredScreenWidth={resolutionWidth}");
                    fileContent = Regex.Replace(fileContent, @"LastUserConfirmedDesiredScreenHeight=.*", $"LastUserConfirmedDesiredScreenHeight={resolutionHeight}");

                    File.WriteAllText(settingsFile, fileContent);

                    Console.WriteLine($"edit completed on file: {settingsFile}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error on edit file: {settingsFile}: {ex.Message}");
                    Console.ReadLine();
                    return;
                }
            }
            else
            {
                Console.WriteLine($"File GameUserSettings.ini not found on {dir}\\Windows");
            }
        }

        Console.WriteLine("Done!, U can open valorant now!");
        Console.ReadLine();
    }
}
