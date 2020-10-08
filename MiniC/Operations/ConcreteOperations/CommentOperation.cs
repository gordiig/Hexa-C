using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class CommentOperation: IOperation
    {
        protected string comment;
        protected bool withTab;

        public string Comment => comment;

        public CommentOperation(string comment, bool withTab = true)
        {
            this.comment = comment;
            this.withTab = withTab;
        }

        public OperationType OperationType => OperationType.NonOp;
        public IOperand Lhs => null;
        public IOperand Rhs => null;
        public string AsmString => $"{(withTab ? "\t" : "")}// {Comment}";
    }
}