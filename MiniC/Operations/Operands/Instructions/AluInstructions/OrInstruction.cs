namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class OrInstruction: AluInstruction
    {
        public OrInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "or";
        }
    }
}