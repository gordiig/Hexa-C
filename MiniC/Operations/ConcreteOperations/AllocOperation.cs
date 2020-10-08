using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Operations.Operands.Instructions.AllocInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class AllocOperation: IOperation
    {
        protected AllocframeInstruction lhs;

        public AllocframeInstruction LhsAsAllocframeInstruction => lhs;

        public AllocOperation(AllocframeInstruction lhs)
        {
            this.lhs = lhs;
        }

        public AllocOperation(IntConstOperand intConstOperand)
        {
            lhs = new AllocframeInstruction(intConstOperand);
        }

        public AllocOperation(int size)
        {
            lhs = new AllocframeInstruction(size);
        }

        public IOperand Lhs => lhs;
        public IOperand Rhs => null;
        public OperationType OperationType => OperationType.ST;

        public string AsmString => $"\t{Lhs.AsmString};";
    }
}