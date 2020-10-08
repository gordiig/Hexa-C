using MiniC.Exceptions;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class OrInstruction: ArithmeticInstruction
    {
        public OrInstruction(RegisterOperand firstOperand, RegisterOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
            if (FirstOperand.OperandType == OperandType.Constant_f || SecondOperand.OperandType == OperandType.Constant_f)
                throw new CodeGenerationException("Can't do bitwise operation on float");
        }

        public RegisterOperand SecondOperandAsRegisterOperand => secondOperand as RegisterOperand;
        
        public override string InstructionString => "or";
    }
}