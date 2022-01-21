using System.Reflection;

[assembly: AssemblyVersion("1.2.2")]
namespace Synkrino;
public class Program
{
    public static void Main()
    {
        #region Uncomment on release
        Console.WriteLine($"Synkrino Ver.{Assembly.GetExecutingAssembly().GetName().Version}\nUse the 'help' or 'credits' command for more information, use the 'exit' command to exit the environment.");
        
        while (true)
        {
            try
            {
                Console.Write(">> ");
                string[] arguements = Console.ReadLine()!.Split(' ', 100);

                switch (arguements[0])
                {
                    case "exit":
                        Environment.Exit(0);
                        break;

                    case "help":
                        Console.WriteLine("Command still in development.");
                        break;

                    case "compare" when (arguements.Length == 4 && arguements[2] == "to") :
                    {    
                        string? file1 = GetFile(arguements[1]);
                        string? file2 = GetFile(arguements[3]);

                        Compare compare = new(file1, file2);
                        ReturnAnalysis(compare.TreeBeingCompared);
                        break;
                    }

                    case "":
                        break;

                    default:
                        Console.WriteLine("Invalid syntax or command!");
                        break;
                }
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("One of the path provided is unreachable. Please make sure both of them are valid.");
                continue;
            }
        }
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
    public static void ReturnAnalysis(DynamicSyntaxTree treeBeingCompared)
    {
        if(!treeBeingCompared.HasDynamicChildren)
        {
            Console.WriteLine("The code has no dissimilarities.");
            return;
        }

        if(treeBeingCompared.HasDynamicChildren)
        {
            Console.WriteLine("The following lines in the reference file have issues:");

            foreach (DynamicSyntaxTree child in treeBeingCompared.Children)
            {
                PrintLine(child);
            }
        }
    }
    public static void PrintLine(DynamicSyntaxTree tree)
    {
        if (!tree.HasDynamicChildren)
        {
            Console.WriteLine(tree.Self!.GetLocation().GetLineSpan().StartLinePosition.Line + 1);
            return;
        }
        
        foreach (DynamicSyntaxTree child in tree.Children)
            PrintLine(child);
    }
}