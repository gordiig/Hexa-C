using System.Text;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations
{
    public class AddGlobalArrayOperation: AddGlobalSymbolOperation
    {
        protected int capacity;

        public int Capacity => capacity;

        public AddGlobalArrayOperation(string name, SymbolType type, int capacity) : 
            base(name, type)
        {
            this.capacity = capacity;
        }

        public override string AsmString
        {
            get
            {
                var ans = new StringBuilder($"\n{name}:\n\t.{type.Name} ");
                for (int i = 0; i < capacity; i++)
                {
                    ans.Append("0, ");
                }
                if (capacity > 0)
                    ans = ans.Remove(ans.Length - 2, 2);
                return ans.ToString();
            }
        }
    }
}