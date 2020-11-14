using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;

namespace MiniC.Operations.ConcreteOperations.AssignOperations
{
    public class ValueToRegisterOperation: AssignOperation
    {
        public ConstOperand RhsAsConstOperand => rhs as ConstOperand;

        public ValueToRegisterOperation(RegisterOperand lhs, ConstOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public ValueToRegisterOperation(Register lhs, ConstOperand rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = rhs;
        }
        
        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}