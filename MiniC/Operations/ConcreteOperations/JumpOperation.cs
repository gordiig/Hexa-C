using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.JumpInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class JumpOperation: IOperation
    {
        protected BaseJumpInstruction lhs;

        public BaseJumpInstruction LhsBaseJumpInstruction => lhs;

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

        public IOperand Lhs => lhs;
        public IOperand Rhs => null;
        public OperationType OperationType => OperationType.J;

        public string AsmString => $"\t{Lhs.AsmString};";
    }
}