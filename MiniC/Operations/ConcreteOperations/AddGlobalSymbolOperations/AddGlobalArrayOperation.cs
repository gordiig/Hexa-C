using System.Text;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations
{
    public class AddGlobalArrayOperation: AddGlobalSymbolOperation
    {
        public int Capacity { get; }

        public AddGlobalArrayOperation(string name, SymbolType type, int capacity) : 
            base(name, type)
        {
            Capacity = capacity;
        }

        public override string OperationAsmString
        {
            get
            {
                var ans = new StringBuilder($"\n{name}:\n\t.{type.Name} ");
                for (int i = 0; i < Capacity; i++)
                {
                    ans.Append("0, ");
                }
                if (Capacity > 0)
                    ans = ans.Remove(ans.Length - 2, 2);
                return ans.ToString();
            }
        }
    }
}