using System.Collections;
using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public interface IOperation: IAsmWritable
    {
        OperationType OperationType { get; }
        IOperand Lhs { get; }
        IOperand Rhs { get; }
        ArrayList LowerComments { get;}
        void AddLowerComment(string comment, bool withTab);
        string InlineComment { get; set; }
        string OperationAsmString { get; }

        string ToString() => AsmString;
    }
}