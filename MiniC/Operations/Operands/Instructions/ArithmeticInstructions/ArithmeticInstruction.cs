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
            // if (firstOperand.OperandType != OperandType.Constant_i && firstOperand.OperandType != OperandType.Constant_f  
            //                                                        && firstOperand.OperandType != OperandType.Register)
            //     throw new CodeGenerationException("First operand of AluInstruction must be either " +
            //                                       "constant or register");
            // if (secondOperand != null && 
            //     (firstOperand.OperandType != OperandType.Constant_i && firstOperand.OperandType != OperandType.Constant_f
            //                                                         && secondOperand.OperandType != OperandType.Register))
            //     throw new CodeGenerationException("Second operand of AluInstruction must be either " +
            //                                       "null, constant or register");
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