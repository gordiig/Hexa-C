using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.JumpInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class JumpOperation: BaseOperation
    {
        // protected BaseJumpInstruction lhs;

        public BaseJumpInstruction LhsBaseJumpInstruction => lhs as BaseJumpInstruction;

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
        
        public override OperationType OperationType => OperationType.J;

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}