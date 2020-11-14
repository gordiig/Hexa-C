using MiniC.Generators;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions
{
    public class MemInstruction: IAsmInstruction
    {

        #region Variables

        protected RegisterOperand address = null;
        protected IntConstOperand offset = null;

        public RegisterOperand FirstOperandAsRegisterOperand => address;
        public IntConstOperand SecondOperandAsIntConstOperand => offset;

        #endregion

        #region Constructors

        public MemInstruction(RegisterOperand address, IntConstOperand offset = null)
        {
            this.address = address;
            this.offset = offset;
        }
        
        public MemInstruction(Register address, string offset = "")
        {
            this.address = new RegisterOperand(address);
            this.offset = offset == "" ? null : new IntConstOperand(offset);
        }

        #endregion

        public OffsetsSet OffsetsSet
        {
            get
            {
                var ans = new OffsetsSet(address.Register.Offsets);
                if (offset != null)
                    ans.Add(offset.IntRepr.ToString());
                return ans;
            }
        }

        #region Interface impl

        public IOperand FirstOperand => address;
        public IOperand SecondOperand => offset;
        public string InstructionString => OperandType == OperandType.Memory_b ? "memb" : "memw";

        public OperandType OperandType =>
            address.Register.Type == SymbolType.GetType("char") ? OperandType.Memory_b : OperandType.Memory_i;

        public string AsmString
        {
            get
            {
                string ans = $"{InstructionString}({FirstOperand.AsmString}";
                if (SecondOperand != null)
                    ans += $" + {SecondOperand.AsmString}";
                ans += ")";
                return ans;
            }
        }

        #endregion

    }
}