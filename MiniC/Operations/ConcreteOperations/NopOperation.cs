namespace MiniC.Operations.ConcreteOperations
{
    public class NopOperation: BaseOperation
    {
        public NopOperation()
        {
        }

        public override OperationType OperationType => OperationType.ALU;
        public override string OperationAsmString => "\tnop;";
    }
}