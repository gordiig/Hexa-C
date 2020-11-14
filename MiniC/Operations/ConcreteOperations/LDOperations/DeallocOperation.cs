using MiniC.Operations.Operands.Instructions.AllocInstructions;

namespace MiniC.Operations.ConcreteOperations.LDOperations
{
    public class DeallocOperation: LDOperation
    {
        public DeallocInstruction LhsAsDeallocOperation => lhs as DeallocInstruction;

        public DeallocOperation(DeallocInstruction lhs)
        {
            this.lhs = lhs;
        }

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}