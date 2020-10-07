using MiniC.Exceptions;

namespace MiniC.Operations.Operands.Instructions.MemInstructions
{
    public abstract class MemInstruction: IAsmInstruction
    {

        #region Variables

        protected IOperand address = null;
        protected IOperand offset = null;
        protected string instructionString = null;

        #endregion

        #region Constructors

        protected MemInstruction(IOperand address, IOperand offset = null)
        {
            if (address.OperandType != OperandType.Constant && address.OperandType != OperandType.Register) 
                throw new CodeGenerationException("MemInstruction address must be either constant or register");
            if (offset != null && offset.OperandType != OperandType.Constant)
                throw new CodeGenerationException("MemInstruction address must be either null or constant");
            this.address = address;
            this.offset = offset;
        }

        #endregion

        #region Interface impl

        public IOperand FirstOperand => address;
        public IOperand SecondOperand => offset;
        public string InstructionString => instructionString;

        public OperandType OperandType => OperandType.Memory;

        public string AsmString
        {
            get
            {
                string ans = $"{InstructionString}(${FirstOperand.AsmString}";
                if (SecondOperand != null)
                    ans += $" + {SecondOperand.AsmString}";
                ans += ")";
                return ans;
            }
        }

        #endregion

    }
}