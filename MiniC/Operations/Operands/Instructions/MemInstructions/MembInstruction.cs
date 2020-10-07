namespace MiniC.Operations.Operands.Instructions.MemInstructions
{
    public class MembInstruction: MemInstruction
    {
        public MembInstruction(IOperand address, IOperand offset = null) : base(address, offset)
        {
            instructionString = "memb";
        }
    }
}