using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations
{
    public abstract class AddGlobalSymbolOperation: BaseOperation
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

        public override OperationType OperationType => OperationType.NonOp;
    }
}