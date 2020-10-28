using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class LabelOperation: BaseOperation
    {
        // protected LabelOperand lhs;

        public LabelOperand LhsAsLabelOperand => lhs as LabelOperand;

        public LabelOperation(LabelOperand lhs)
        {
            this.lhs = lhs;
        }

        public LabelOperation(string label)
        {
            lhs = new LabelOperand(label);
        }
        
        public override OperationType OperationType => OperationType.NonOp;

        public override string OperationAsmString => $"{Lhs.AsmString}:";
    }
}