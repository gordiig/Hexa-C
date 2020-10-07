namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class AddInstruction: AluInstruction
    {
        public AddInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "add";
        }
    }
}