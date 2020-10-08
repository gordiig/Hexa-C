using MiniC.Exceptions;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class AndInstruction: ArithmeticInstruction
    {
        public AndInstruction(RegisterOperand firstOperand, RegisterOperand secondOperand = null) 
            : base(firstOperand, secondOperand)
        {
            if (firstOperand.OperandType == OperandType.Constant_f || secondOperand.OperandType == OperandType.Constant_f)
                throw new CodeGenerationException("Can't do bitwise operation on float");
        }

        public RegisterOperand SecondOperandAsRegisterOperand => secondOperand as RegisterOperand;
        
        public override string InstructionString => "and";
    }
}