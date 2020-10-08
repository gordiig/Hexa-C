namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class RShiftInstruction: ArithmeticInstruction
    {
        public RShiftInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
        }

        public override string InstructionString => "lsr";
    }
}