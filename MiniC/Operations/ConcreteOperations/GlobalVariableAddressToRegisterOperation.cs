using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations
{
    public class GlobalVariableAddressToRegisterOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected IntConstOperand rhs;

        public GlobalVariableAddressToRegisterOperation(RegisterOperand lhs, IntConstOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }
        
        public GlobalVariableAddressToRegisterOperation(Register lhs, VarSymbol rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = new IntConstOperand(rhs.BaseAddress);
        }

        public OperationType OperationType => OperationType.Assign;
        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;

        public string AsmString => $"{Lhs.AsmString} = ##{Rhs.AsmString}";
    }
}