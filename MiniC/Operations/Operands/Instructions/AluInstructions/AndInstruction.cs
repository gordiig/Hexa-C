namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class AndInstruction: AluInstruction
    {
        public AndInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "and";
        }
    }
}