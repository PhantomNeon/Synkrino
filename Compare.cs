global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.Text;
global using Microsoft.CodeAnalysis.Diagnostics;
global using Microsoft.CodeAnalysis.Formatting;
global using Microsoft.CodeAnalysis.CSharp.Formatting;
global using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Synkrino;
public class Compare
{
    private readonly string? fileBeingCompared;
    private readonly string? fileBeingComparedTo;
    private DynamicSyntaxTree? treeBeingCompared;
    private DynamicSyntaxTree? treeBeingComparedTo;

    public DynamicSyntaxTree TreeBeingCompared { get{return treeBeingCompared!;} }
    public DynamicSyntaxTree TreeBeingComparedTo { get{return treeBeingComparedTo!;} }

    public Compare(string? _fileToBeCompared, string? _fileToBeComparedTo)
    {
        fileBeingCompared = _fileToBeCompared;
        fileBeingComparedTo = _fileToBeComparedTo;
        Comparison();
    }

    public void Comparison()
    {
        SyntaxTree tree1 = CSharpSyntaxTree.ParseText(fileBeingCompared!);
        SyntaxNode root1 = tree1.GetCompilationUnitRoot();
        treeBeingCompared = DynamicSyntaxTree.CreateTree(root1);

        SyntaxTree tree2 = CSharpSyntaxTree.ParseText(fileBeingComparedTo!);
        SyntaxNode root2 = tree2.GetCompilationUnitRoot();
        treeBeingComparedTo = DynamicSyntaxTree.CreateTree(root2);

        FilterTrees(ref treeBeingCompared, ref treeBeingComparedTo);
    }
    private bool FilterTrees(ref DynamicSyntaxTree list1, ref DynamicSyntaxTree list2)
    {
        if(!list1.HasDynamicChildren && !list2.HasDynamicChildren)
            return list1.Self!.IsEquivalentTo(list2.Self);

        for (int i = 0; i < list1.Children.Count; i++)
        {
            for (int n = 0; n < list2.Children.Count; n++)
            {
                if (FilterTrees(list1.Children[i], list2.Children[n], out DynamicSyntaxTree output1, out DynamicSyntaxTree output2))
                {
                    list1.Children.Remove(list1.Children[i]);
                    list2.Children.Remove(list2.Children[n]);
                    i--;
                    break;
                }
                else
                {
                    list1.Children[i] = output1;
                    list2.Children[n] = output2;
                }
            }
        }

        if ((list1.Children.Count == 0) && (list2.Children.Count == 0))
            return true;
        else
            return false;
    }
    private bool FilterTrees(DynamicSyntaxTree list1, DynamicSyntaxTree list2, out DynamicSyntaxTree list1Output, out DynamicSyntaxTree list2Output)
    {
        list1Output = new DynamicSyntaxTree();
        list2Output = new DynamicSyntaxTree();

        if(!list1.HasDynamicChildren && !list2.HasDynamicChildren)
            return list1.Self!.IsEquivalentTo(list2.Self);

        for (int i = 0; i < list1.Children.Count; i++)
        {
            for (int n = 0; n < list2.Children.Count; n++)
            {
                if (FilterTrees(list1.Children[i], list2.Children[n], out DynamicSyntaxTree output1, out DynamicSyntaxTree output2))
                {
                    list1.Children.Remove(list1.Children[i]);
                    list2.Children.Remove(list2.Children[n]);
                    i--;
                    break;
                }
                else
                {
                    list1.Children[i] = output1;
                    list2.Children[n] = output2;
                }
            }
        }

        list1Output = list1;
        list2Output = list2;

        if ((list1.Children.Count == 0) && (list2.Children.Count == 0))
            return true;
        else
            return false;
    }
}