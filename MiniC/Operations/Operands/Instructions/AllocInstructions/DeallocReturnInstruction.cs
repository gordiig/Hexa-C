namespace MiniC.Operations.Operands.Instructions.AllocInstructions
{
    public class DeallocReturnInstruction: IAsmInstruction
    {
        public IOperand FirstOperand => null;
        public IOperand SecondOperand => null;
        public OperandType OperandType => OperandType.Instruction;
        public string InstructionString => "dealloc_return";

        public string AsmString => InstructionString;
    }
}