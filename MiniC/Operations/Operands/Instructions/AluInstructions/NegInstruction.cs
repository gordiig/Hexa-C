namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class NegInstruction: AluInstruction
    {
        public NegInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "neg";
        }
    }
}