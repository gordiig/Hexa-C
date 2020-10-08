namespace MiniC.Operations.Operands.Instructions.AllocInstructions
{
    public abstract class DeallocInstruction: IAsmInstruction
    {
        public IOperand FirstOperand => null;
        public IOperand SecondOperand => null;
        public OperandType OperandType => OperandType.Instruction;
        
        public virtual string InstructionString { get; }
        
        public virtual string AsmString => InstructionString;
    }
}