namespace MiniC.Generators.CodeOptimizers
{
    public interface IOptimizable
    {
        public ListingScopeTreeDir.ListingScopeTree Optimize(ICodeOptimizer optimizer);
    }
}