using System.Text;
using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public abstract class BaseOperation: IOperation
    {
        protected IOperand lhs = null;
        protected IOperand rhs = null;

        protected string upperComment = null;
        protected bool upperCommentWithTab = false;

        public virtual IOperand Lhs => lhs;
        public virtual IOperand Rhs => rhs;

        public string UpperComment => upperComment;
        public string InlineComment { get; set; }

        public abstract OperationType OperationType { get; }
        public abstract string OperationAsmString { get; }

        public void SetUpperComment(string comment, bool withTab)
        {
            upperComment = comment;
            upperCommentWithTab = withTab;
        }

        public string AsmString
        {
            get
            {
                var ans = new StringBuilder();
                if (!string.IsNullOrEmpty(UpperComment))
                    ans.AppendLine($"{(upperCommentWithTab ? "\t" : "")}// {UpperComment}");
                ans.Append(OperationAsmString);
                if (!string.IsNullOrEmpty(InlineComment))
                    ans.Append($"  // {InlineComment}");
                return ans.ToString();
            }
        }
    }
}