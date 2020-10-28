using MiniC.Operations.Operands.Instructions.AllocInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class DeallocOperation: BaseOperation
    {
        // protected DeallocInstruction lhs;

        public DeallocInstruction LhsAsDeallocOperation => lhs as DeallocInstruction;

        public DeallocOperation(DeallocInstruction lhs)
        {
            this.lhs = lhs;
        }
        
        public override OperationType OperationType => OperationType.LD;

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}