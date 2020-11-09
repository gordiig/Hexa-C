namespace MiniC.Operations.Operands.Instructions.JumpInstructions
{
    public class JumpInstruction: BaseJumpInstruction
    {
        public JumpInstruction(string label) : base(label)
        {
        }

        public JumpInstruction(LabelOperand label) : base(label)
        {
        }

        public override string InstructionString => "jump";
    }
}