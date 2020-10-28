using System.Collections;
using System.Text;
using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public abstract class BaseOperation: IOperation
    {
        protected IOperand lhs = null;
        protected IOperand rhs = null;

        protected ArrayList lowerComments = new ArrayList();
        protected bool LowerCommentWithTab = false;

        public virtual IOperand Lhs => lhs;
        public virtual IOperand Rhs => rhs;

        public ArrayList LowerComments => lowerComments;
        public string InlineComment { get; set; }

        public abstract OperationType OperationType { get; }
        public abstract string OperationAsmString { get; }

        public void AddLowerComment(string comment, bool withTab)
        {
            lowerComments.Add(comment);
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
                foreach (string comment in LowerComments)
                    ans.Append($"\n{(LowerCommentWithTab ? "\t" : "")}// {comment}");
                return ans.ToString();
            }
        }
    }
}