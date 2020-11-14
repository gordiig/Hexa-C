using MiniC.Operations.Operands.Instructions.JumpInstructions;

namespace MiniC.Operations.ConcreteOperations.JOperations
{
    public abstract class JOperation: BaseOperation
    {
        public BaseJumpInstruction LhsBaseJumpInstruction => lhs as BaseJumpInstruction;
        
        public override OperationType OperationType => OperationType.J;
    }
}