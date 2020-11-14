using MiniC.Generators.ListingScopeTreeDir;

namespace MiniC.Generators.CodeOptimizers
{
    public interface ICodeOptimizer
    {
        public ListingScopeTree Optimize(ListingScopeTree scopeTree);
    }
}