using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.JumpInstructions;

namespace MiniC.Operations.ConcreteOperations.JOperations
{
    public class JumpOperation: JOperation
    {
        public JumpOperation(BaseJumpInstruction lhs)
        {
            this.lhs = lhs;
        }

        public JumpOperation(LabelOperand labelOperand, bool call = false)
        {
            lhs = call ? (BaseJumpInstruction) new CallInstruction(labelOperand) : new JumpInstruction(labelOperand);
        }

        public JumpOperation(string label, bool call = false)
        {
            lhs = call ? (BaseJumpInstruction) new CallInstruction(label) : new JumpInstruction(label);
        }

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}