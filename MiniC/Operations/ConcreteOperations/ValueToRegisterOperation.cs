using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;

namespace MiniC.Operations.ConcreteOperations
{
    public class ValueToRegisterOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected ConstOperand rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs;
        public ConstOperand RhsAsConstOperand => rhs;

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

        public OperationType OperationType => OperationType.Assign;
        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;
        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}