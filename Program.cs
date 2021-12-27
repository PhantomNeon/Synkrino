using System.Reflection;

[assembly: AssemblyVersion("2.0.0")]
namespace Synkrino;
public class Program
{
    public static void Main()
    {
        Compare compare = new(GetFile(@"D:\Stuff\More Stuff\Synkrino\test.txt"), GetFile(@"D:\Stuff\More Stuff\Synkrino\test2.txt"));

        PrintTree(compare.TreeBeingCompared);
        Console.WriteLine("\n\n");
        PrintTree(compare.TreeBeingComparedTo);

        #region Uncomment on release
        // string path1;
        // string path2;

        // Console.WriteLine($"Synkrino Ver.{Assembly.GetExecutingAssembly().GetName().Version}\nUse the 'help' or 'credits' command for more information, use the 'exit' command to exit the environment.");
        // while (true)
        // {
        //     Console.Write(">> ");
        //     string[] arguments = Console.ReadLine()!.Split(' ', 100);

        //     if (arguments.Length != 0)
        //     {
        //         if (arguments[0] == "exit")
        //             Environment.Exit(0);

        //         else if (arguments[0] == "help")
        //         {
        //             if (arguments.Length == 1)
        //                 Console.WriteLine("Use the 'compare' command to compare two files.\nSyntax:\ncompare <Path to your file in qoutes> to <Path to the file to be compared with in quotes>\nType 'exit' to exit the environment.");
        //             //To be written.
        //             else if (arguments.Length == 2)
        //             {
        //                 switch (arguments[1])
        //                 {
        //                     default:
        //                         Console.WriteLine("The command you are trying to use is currently unfinished.\nType 'exit' to exit the environment.");
        //                         break;
        //                 }
        //             }
        //             //To be implemented.
        //             else
        //                 Console.WriteLine("Unidentified command, please make sure if you spelled everything correctly.\nType 'exit' to exit the environment.");
        //         }

        //         else if (arguments[0] == "credits")
        //             Console.WriteLine("Created by PhantomNeon.\nType 'exit' to exit the environment.");

        //         else if (arguments[0] == "compare")
        //         {
        //             if (arguments.Length == 4)
        //             {
        //                 path1 = arguments[1];
        //                 if (arguments[2] == "to")
        //                 {
        //                     path2 = arguments[3];
        //                     compare.Comparison(GetFile(path1), GetFile(path2));
        //                 }

        //                 else
        //                     Console.WriteLine("Unidentified command, please make sure if you spelled everything correctly.\nType 'exit' to exit the environment.");
        //             }

        //             else
        //                 Console.WriteLine("Unidentified command, please make sure if you spelled everything correctly.\nType 'exit' to exit the environment.");
        //         }

        //         else if(arguments[0] == "")
        //             continue;

        //         else
        //             Console.WriteLine("Unidentified command, please make sure if you spelled everything correctly.\nType 'exit' to exit the environment.");
        //     }
        // }
        #endregion
    }

    public static string? GetFile(string path)
    {
        if(path == null)
            return "";

        string finalData = $"";
        string[] data = File.ReadAllLines(path);

        foreach (string line in data)
            finalData += line + "\n";

        return finalData;
    }
    public static void PrintTree(DynamicSyntaxTree tree)
    {
        Console.WriteLine(tree.Self);
        if (tree.HasChildren)
        {
            foreach (DynamicSyntaxTree item in tree.Children!)
            {
                PrintTree(item);
            }
        }
    }
}