namespace MiniC.Generators.CodeOptimizers
{
    public interface ICodeOptimizer
    {
        public ListingScopeTreeDir.ListingScopeTree Optimize(ListingScopeTreeDir.ListingScopeTree scopeTree);
    }
}