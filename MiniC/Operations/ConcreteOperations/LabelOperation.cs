using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class LabelOperation: IOperation
    {
        protected LabelOperand lhs;

        public LabelOperand LhsAsLabelOperand => lhs;

        public LabelOperation(LabelOperand lhs)
        {
            this.lhs = lhs;
        }

        public LabelOperation(string label)
        {
            lhs = new LabelOperand(label);
        }

        public IOperand Lhs => lhs;
        public IOperand Rhs => null;
        public OperationType OperationType => OperationType.NonOp;

        public string AsmString => $"{Lhs.AsmString}:";
    }
}