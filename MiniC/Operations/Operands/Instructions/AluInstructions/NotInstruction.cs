namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class NotInstruction: AluInstruction
    {
        public NotInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "not";
        }
    }
}