using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public interface IOperation: IAsmWritable
    {
        OperationType OperationType { get; }
        IOperand Lhs { get; }
        IOperand Rhs { get; }
        string UpperComment { get;}
        void SetUpperComment(string comment, bool withTab);
        string InlineComment { get; set; }
        string OperationAsmString { get; }

        string ToString() => AsmString;
    }
}