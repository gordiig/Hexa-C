namespace MiniC.Operations.Operands.Instructions.MemInstructions
{
    public class MemwInstruction: MemInstruction
    {
        public MemwInstruction(IOperand address, IOperand offset = null) : base(address, offset)
        {
            instructionString = "memw";
        }
    }
}