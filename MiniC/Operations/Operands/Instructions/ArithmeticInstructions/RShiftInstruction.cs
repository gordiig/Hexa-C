using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class RShiftInstruction: ArithmeticInstruction
    {
        public RShiftInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
        }
        
        public RShiftInstruction(Register firstOperand, Register secondOperand = null) : 
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
        {
        }

        public override string InstructionString => "lsr";
    }
}