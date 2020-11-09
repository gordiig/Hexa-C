namespace MiniC.Operations.Operands.Instructions.JumpInstructions
{
    public class CallInstruction: BaseJumpInstruction
    {
        public CallInstruction(string label) : base(label)
        {
        }

        public CallInstruction(LabelOperand label) : base(label)
        {
        }
        
        public override string InstructionString => "call";
    }
}