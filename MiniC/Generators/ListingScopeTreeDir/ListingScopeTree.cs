using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniC.Generators.CodeOptimizers;
using MiniC.Operations;

namespace MiniC.Generators.ListingScopeTreeDir
{
    public class ListingScopeTree: IAsmWritable, IOptimizable
    {
        // Only IOperation or ListingScopeTree (meaning all entries are IAsmWritable)
        private ArrayList scopeEntries;
        private string name;
        private ListingScopeType scopeType;

        public string Name => name;
        public ListingScopeType ScopeType => scopeType;

        public ListingScopeTree(string name, ListingScopeType type)
        {
            scopeEntries = new ArrayList();
            this.name = name;
            scopeType = type;
        }

        public void Add(IOperation op)
        {
            scopeEntries.Add(op);
        }
        public void Add(ListingScopeTree scope)
        {
            scopeEntries.Add(scope);
        }

        public List<IOperation> AsPlainOperations()
        {
            var list = new List<IOperation>();
            foreach (var scopeEntry in scopeEntries)
            {
                if (scopeEntry is IOperation operationEntry)
                    list.Add(operationEntry);
                else if (scopeEntry is ListingScopeTree scopeTreeEntry)
                    list.AddRange(scopeTreeEntry.AsPlainOperations());
                else
                    throw new InvalidOperationException();
            }
            return list;
        }

        public List<IOperation> OperationsOnly()
        {
            var query = from object obj in scopeEntries 
                where obj is IOperation 
                select (IOperation) obj;
            return new List<IOperation>(query);
        }
        
        public List<ListingScopeTree> ScopesOnly()
        {
            var query = from object obj in scopeEntries 
                where obj is ListingScopeTree scopeObj 
                select (ListingScopeTree) obj;
            return new List<ListingScopeTree>(query);
        }
        public List<ListingScopeTree> ScopesOnly(ListingScopeType type)
        {
            var query = from object obj in scopeEntries 
                where obj is ListingScopeTree scopeObj && scopeObj.ScopeType == type
                select (ListingScopeTree) obj;
            return new List<ListingScopeTree>(query);
        }
        
        public string AsmString
        {
            get
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < scopeEntries.Count - 1; i++)
                    stringBuilder.AppendLine(((IAsmWritable) scopeEntries[i])?.AsmString);
                stringBuilder.Append(((IAsmWritable) scopeEntries[^1])?.AsmString);
                return stringBuilder.ToString();
            }
        }

        public ListingScopeTree Optimize(ICodeOptimizer optimizer)
        {
            return optimizer.Optimize(this);
        }
    }
}