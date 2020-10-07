namespace MiniC.Operations.Operands.Instructions.AluInstructions
{
    public class XorInstruction: AluInstruction
    {
        public XorInstruction(IOperand firstOperand, IOperand secondOperand = null) : base(firstOperand, secondOperand)
        {
            instructionString = "xor";
        }
    }
}