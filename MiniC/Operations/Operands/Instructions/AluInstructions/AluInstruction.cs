using MiniC.Exceptions;

namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public abstract class AluInstruction: IAsmInstruction
    {
        #region Variables

        protected IOperand firstOperand = null;
        protected IOperand secondOperand = null;
        protected string instructionString = null;

        #endregion

        #region Constructors

        public AluInstruction(IOperand firstOperand, IOperand secondOperand = null)
        {
            if (firstOperand.OperandType != OperandType.Constant && firstOperand.OperandType != OperandType.Register)
                throw new CodeGenerationException("First operand of AluInstruction must be either " +
                                                  "constant or register");
            if (secondOperand != null && 
                (secondOperand.OperandType != OperandType.Constant && secondOperand.OperandType != OperandType.Register))
                throw new CodeGenerationException("Second operand of AluInstruction must be either " +
                                                  "null, constant or register");
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
        }

        #endregion

        #region Interface impl

        public IOperand FirstOperand => firstOperand;
        public IOperand SecondOperand => secondOperand;
        public string InstructionString => instructionString;

        public OperandType OperandType => OperandType.Instruction;

        public string AsmString
        {
            get
            {
                string ans = $"{InstructionString}({FirstOperand.AsmString}";
                if (SecondOperand != null)
                    ans += $", {SecondOperand.AsmString}";
                ans += ")";
                return ans;
            }
        }

        #endregion


    }
}