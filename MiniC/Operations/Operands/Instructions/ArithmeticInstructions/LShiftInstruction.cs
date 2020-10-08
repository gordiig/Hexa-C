using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class LShiftInstruction: ArithmeticInstruction
    {
        public LShiftInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }
        
        public LShiftInstruction(Register firstOperand, Register secondOperand = null) :
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
        {
        }

        public override string InstructionString => "lsl";
    }
}