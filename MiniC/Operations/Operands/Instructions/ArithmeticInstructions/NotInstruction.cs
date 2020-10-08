using MiniC.Exceptions;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class NotInstruction: ArithmeticInstruction
    {
        public NotInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
            if (FirstOperand.OperandType == OperandType.Constant_f)
                throw new CodeGenerationException("Can't do bitwise operation on float");
        }

        public override string InstructionString => "not";
    }
}