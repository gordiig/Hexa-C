using MiniC.Exceptions;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public abstract class ArithmeticInstruction: IAsmInstruction
    {
        #region Variables

        protected RegisterOperand firstOperand = null;
        protected IOperand secondOperand = null;

        public RegisterOperand FirstOperandAsRegisterOperand => firstOperand;
        public virtual ArithmeticInstructionType ArithmeticInstructionType => ArithmeticInstructionType.Int;

        #endregion

        #region Constructors

        public ArithmeticInstruction(RegisterOperand firstOperand, IOperand secondOperand = null)
        {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
        }

        #endregion

        #region Interface impl

        public IOperand FirstOperand => firstOperand;
        public IOperand SecondOperand => secondOperand;
        public virtual string InstructionString { get; }

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