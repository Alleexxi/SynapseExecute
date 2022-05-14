using System;
using WebSocketSharp;
using Microsoft.Win32;
class TestClass
{
    private static RegistryKey BaseFolderPath;
    static void Main(string[] args)
    {
        string xd = System.Reflection.Assembly.GetExecutingAssembly().Location;
        if (args.Length == 0)
        {
            BaseFolderPath = Registry.ClassesRoot.OpenSubKey("*",true);
            RegistryKey RegKey = BaseFolderPath.CreateSubKey("shell", true);
            RegistryKey RegKey2 = RegKey.CreateSubKey("SynapseExecute",true);
            RegistryKey command = RegKey2.CreateSubKey("command", true);
            RegKey2.SetValue("", "Execute with Synapse");
            RegKey2.SetValue("Icon", "\""+ xd.Replace(".dll", ".exe")  + "\"");

            command.SetValue("", "\""+ xd.Replace(".dll", ".exe")  + "\" \"%1\"");
            RegKey2.Flush();
            RegKey.Flush();
            command.Flush();
            System.Environment.Exit(0);
            return;
        }
        string script = System.IO.File.ReadAllText(args[0]);
        Console.WriteLine(script);
        using (var ws = new WebSocket("ws://localhost:24892/execute"))
        {

            ws.Connect();
            ws.Send(script);
            Console.ReadKey(true);
            ws.Close();
        }
    }
}