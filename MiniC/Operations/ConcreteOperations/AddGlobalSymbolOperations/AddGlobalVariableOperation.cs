using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations
{
    public class AddGlobalVariableOperation: AddGlobalSymbolOperation
    {
        public AddGlobalVariableOperation(string name, SymbolType type) : 
            base(name, type)
        {
        }

        public override string AsmString => $"{name}:\n\t.{type.Name}\t0";
    }
}