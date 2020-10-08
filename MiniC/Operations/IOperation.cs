using MiniC.Operations.Operands;

namespace MiniC.Operations
{
    public interface IOperation: IAsmWritable
    {
        OperationType OperationType { get; }
        IOperand Lhs { get; }
        IOperand Rhs { get; }

        string ToString() => AsmString;
    }
}