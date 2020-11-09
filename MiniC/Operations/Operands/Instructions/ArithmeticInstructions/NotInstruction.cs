using MiniC.Exceptions;
using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class NotInstruction: ArithmeticInstruction
    {
        public NotInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
        }
        
        public NotInstruction(Register firstOperand, IOperand secondOperand = null) : 
            base(new RegisterOperand(firstOperand), secondOperand)
        {
        }

        public override string InstructionString => "not";
    }
}