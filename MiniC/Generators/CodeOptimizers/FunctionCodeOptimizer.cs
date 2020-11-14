using System;
using MiniC.Generators.ListingScopeTreeDir;
using MiniC.Operations;

namespace MiniC.Generators.CodeOptimizers
{
    public class FunctionCodeOptimizer: BaseCodeOptimizer
    {
        public override ListingScopeTree Optimize(ListingScopeTree scopeTree)
        {
            // Результат работы -- оптимизированный код
            var result = new ListingScopeTree(scopeTree.Name, scopeTree.ScopeType);
            
            // Проход по всем элементам кода 
            foreach (var entry in scopeTree.ScopeEntries)
            {
                switch (entry)
                {
                    // Если это обычная операция, то просто добавляем ее в результат
                    case IOperation operation:
                        result.Add(operation);
                        break;
                    // Если это дерево, и оно является функцией -- то оптимизируем, иначе просто добавляем
                    case ListingScopeTree tree:
                        result.Add(tree.ScopeType == ListingScopeType.Func
                            ? OptimizeJob(tree.AsPlainOperations(), tree.Name, tree.ScopeType)
                            : tree);
                        break;
                    // Такой ситуаци не может быть
                    default:
                        throw new InvalidOperationException();
                }
            }

            return result;
        }
    }
}