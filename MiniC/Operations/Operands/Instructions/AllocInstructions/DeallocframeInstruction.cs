namespace MiniC.Operations.Operands.Instructions.AllocInstructions
{
    public class DeallocframeInstruction: IAsmInstruction
    {
        public IOperand FirstOperand => null;
        public IOperand SecondOperand => null;
        public OperandType OperandType => OperandType.Instruction;
        public string InstructionString => "deallocframe";

        public string AsmString => InstructionString;
    }
}