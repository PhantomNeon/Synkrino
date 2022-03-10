using System.Reflection;
using System.Net;

[assembly: AssemblyVersion("1.3.5")]
namespace Synkrino;
public class Program
{
    public static readonly string HelpText = "Available Commands:\n     compare:     Does comparison.\n     exit:     tExits the Synkrino environment.";

    public static void Main()
    {
        #region Uncomment on release
        Console.WriteLine($"Synkrino Ver. {Assembly.GetExecutingAssembly().GetName().Version}\nUse the 'help' or 'credits' command for more information, use the 'exit' command to exit the environment.");
        
        while (true)
        {
            try
            {
                Console.Write(">>> ");
                string[] arguements = Console.ReadLine()!.Split(' ', 100);

                switch (arguements[0])
                {
                    case "exit":
                        Environment.Exit(0);
                        break;

                    case "help" when arguements.Length == 1:
                        Console.WriteLine(HelpText);
                        break;

                    case "help" when arguements.Length == 2:
                    {
                        switch (arguements[1])
                        {
                            case "compare":
                                Console.WriteLine("Raid Shadow Legends is a-");
                                break;
                            
                            default:
                                EncounterInvalidCommand();
                                break;
                        }
                        break;
                    }

                    case "compare" when arguements.Length == 4 && arguements[2] == "to" :
                    {    
                        string? file1 = GetFileFromPath(arguements[1]);
                        string? file2 = GetFileFromPath(arguements[3]);

                        Compare compare = new(file1, file2);
                        ReturnAnalysis(compare.TreeBeingCompared, compare.TreeBeingComparedTo);
                        break;
                    }

                    case "compare" when arguements.Length == 4 && arguements[2] == "from" :
                    {    
                        string? file1 = GetFileFromPath(arguements[1]);
                        string? file2;

                        using(WebClient client = new())
                            file2 = client.DownloadString(arguements[3]);

                        Compare compare = new(file1, file2);
                        ReturnAnalysis(compare.TreeBeingCompared, compare.TreeBeingComparedTo);
                        break;
                    }

                    case "":
                        break;

                    default:
                        EncounterInvalidCommand();
                        break;
                }
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("One of the path provided is unreachable. Please make sure both of them are valid.");
                continue;
            }

            catch (Exception)
            {
                Console.WriteLine("Something unexpected went wrong. Please try the command again. If it doesn't work again, please file an issue on the github repo");
                continue;
            }
        }
        #endregion
    }
    private static void EncounterInvalidCommand()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid syntax or command!");
        Console.ResetColor();
    }
    private static string? GetFileFromPath(string path)
    {
        if(path == null)
            return "";

        string finalData = $"";
        string[] data = File.ReadAllLines(path);

        foreach (string line in data)
            finalData += line + "\n";

        return finalData;
        
    }
    private static void ReturnAnalysis(DynamicSyntaxTree treeBeingCompared, DynamicSyntaxTree treeBeingComparedTo)
    {
        if(!treeBeingCompared.HasDynamicChildren && !treeBeingComparedTo.HasDynamicChildren)
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

        if(treeBeingComparedTo.HasDynamicChildren)
        {
            Console.WriteLine("The following lines are missing from the reference file:");

            foreach (DynamicSyntaxTree child in treeBeingCompared.Children)
            {
                PrintLine(child);
            }
        }
    }
    private static void PrintLine(DynamicSyntaxTree tree)
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