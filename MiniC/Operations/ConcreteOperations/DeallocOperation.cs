using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.AllocInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class DeallocOperation: IOperation
    {
        protected DeallocInstruction lhs;

        public DeallocInstruction LhsAsDeallocOperation => lhs;

        public DeallocOperation(DeallocInstruction lhs)
        {
            this.lhs = lhs;
        }

        public IOperand Lhs => lhs;
        public IOperand Rhs => null;
        public OperationType OperationType => OperationType.LD;

        public string AsmString => $"\t{Lhs.AsmString};";
    }
}