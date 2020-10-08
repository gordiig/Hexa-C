using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations.SectionOperation
{
    public abstract class SectionOperation: IOperation
    {
        public OperationType OperationType => OperationType.NonOp;
        public IOperand Lhs => null;
        public IOperand Rhs => null;

        public abstract SectionType SectionType { get; }
        
        public string AsmString => $"\t.section\t.{(SectionType == SectionType.Data ? "data" : "text")}";
    }
}