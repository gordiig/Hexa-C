namespace MiniC.Operations.ConcreteOperations.SectionOperation
{
    public abstract class SectionOperation: BaseOperation
    {
        public override OperationType OperationType => OperationType.NonOp;

        public abstract SectionType SectionType { get; }
        
        public override string OperationAsmString => 
            $"\t.section\t.{(SectionType == SectionType.Data ? "data" : "text")}";
    }
}