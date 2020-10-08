using MiniC.Operations.Operands;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations
{
    public abstract class AddGlobalSymbolOperation: IOperation
    {
        protected string name;
        protected SymbolType type;

        public string Name => name;
        public SymbolType Type => type;

        protected AddGlobalSymbolOperation(string name, SymbolType type)
        {
            this.name = name;
            this.type = type;
        }

        public OperationType OperationType => OperationType.NonOp;
        public IOperand Lhs => null;
        public IOperand Rhs => null;
        
        public abstract string AsmString { get; }
    }
}