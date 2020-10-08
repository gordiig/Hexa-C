using MiniC.Generators;
using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class RegisterToRegisterOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected RegisterOperand rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs;
        public RegisterOperand RhsAsRegisterOperand => rhs;

        public RegisterToRegisterOperation(RegisterOperand lhs, RegisterOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public RegisterToRegisterOperation(Register lhs, Register rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = new RegisterOperand(rhs);
        }

        public OperationType OperationType => OperationType.Assign;
        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;
        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}