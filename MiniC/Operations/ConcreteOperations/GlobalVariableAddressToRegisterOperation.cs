using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations
{
    public class GlobalVariableAddressToRegisterOperation: BaseOperation
    {
        // protected RegisterOperand lhs;
        // protected IntConstOperand rhs;
        
        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public IntConstOperand RhsAsIntConstOperand => lhs as IntConstOperand;

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

        public override OperationType OperationType => OperationType.Assign;

        public override string OperationAsmString => $"{Lhs.AsmString} = ##{Rhs.AsmString}";
    }
}