using System.Text;
using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public abstract class BaseOperation: IOperation
    {
        protected IOperand lhs = null;
        protected IOperand rhs = null;

        protected string upperComment = null;
        protected bool LowerCommentWithTab = false;

        public virtual IOperand Lhs => lhs;
        public virtual IOperand Rhs => rhs;

        public string LowerComment => upperComment;
        public string InlineComment { get; set; }

        public abstract OperationType OperationType { get; }
        public abstract string OperationAsmString { get; }

        public void SetLowerComment(string comment, bool withTab)
        {
            upperComment = comment;
            LowerCommentWithTab = withTab;
        }

        public string AsmString
        {
            get
            {
                var ans = new StringBuilder();
                ans.Append(OperationAsmString);
                if (!string.IsNullOrEmpty(InlineComment))
                    ans.Append($"  // {InlineComment}");
                if (!string.IsNullOrEmpty(LowerComment))
                    ans.Append($"\n{(LowerCommentWithTab ? "\t" : "")}// {LowerComment}");
                return ans.ToString();
            }
        }
    }
}