namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class LShiftInstruction: ArithmeticInstruction
    {
        public LShiftInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }

        public override string InstructionString => "lsl";
    }
}