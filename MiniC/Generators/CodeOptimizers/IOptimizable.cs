using MiniC.Generators.ListingScopeTreeDir;

namespace MiniC.Generators.CodeOptimizers
{
    public interface IOptimizable
    {
        public ListingScopeTree Optimize(ICodeOptimizer optimizer);
    }
}