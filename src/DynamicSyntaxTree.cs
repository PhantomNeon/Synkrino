namespace Synkrino;
public class DynamicSyntaxTree
{
    public SyntaxNode? Self { get; init; }
    public List<DynamicSyntaxTree> Children { get; set; } = new();
    public bool HasDynamicChildren { get {return Children.Any();} }
    public bool HasChildren { get{return Self!.ChildNodes().Any();} } 
    public static DynamicSyntaxTree CreateTree(SyntaxNode tree)
    {
        if (!tree.ChildNodes().Any())
            return new DynamicSyntaxTree {Self = tree};
        
        List<DynamicSyntaxTree> Children = new List<DynamicSyntaxTree>()!;

        foreach (SyntaxNode child in tree.ChildNodes())
            Children.Add(CreateTree(child));
        
        return new DynamicSyntaxTree {Self = tree, Children = Children};
    }   
}