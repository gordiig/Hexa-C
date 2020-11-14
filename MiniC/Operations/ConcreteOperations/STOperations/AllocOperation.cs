using MiniC.Operations.Operands.ConstOperands;
using MiniC.Operations.Operands.Instructions.AllocInstructions;

namespace MiniC.Operations.ConcreteOperations.STOperations
{
    public class AllocOperation: STOperation
    {
        public AllocframeInstruction LhsAsAllocframeInstruction => lhs as AllocframeInstruction;

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

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}