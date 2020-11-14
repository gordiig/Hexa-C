using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations.AssignOperations
{
    public abstract class AssignOperation: BaseOperation
    {
        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        
        public override OperationType OperationType => OperationType.Assign;
    }
}