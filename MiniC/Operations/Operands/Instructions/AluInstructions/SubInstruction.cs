namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class SubInstruction: AluInstruction
    {
        public SubInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "sub";
        }
    }
}