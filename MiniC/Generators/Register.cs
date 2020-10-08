using MiniC.Scopes;

namespace MiniC.Generators
{
    public class Register
    {
        public string Name;
        public bool IsFree = true;
        public SymbolType Type = SymbolType.GetType("int");

        public Register(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static Register SP()
        {
            var register = new Register("SP");
            return register;
        }

        public Register Copy()
        {
            var ans = new Register(Name);
            ans.IsFree = IsFree;
            ans.Type = SymbolType.GetType(Type.Name);
            return ans;
        }
    }
}