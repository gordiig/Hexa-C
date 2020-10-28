using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations
{
    public class GlobalVariableAddressToRegisterOperation: BaseOperation
    {
        // protected RegisterOperand lhs;
        // protected LabelOperand rhs;
        
        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public LabelOperand RhsAsIntConstOperand => lhs as LabelOperand;

        public GlobalVariableAddressToRegisterOperation(RegisterOperand lhs, LabelOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }
        
        public GlobalVariableAddressToRegisterOperation(Register lhs, VarSymbol rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = new LabelOperand(rhs.BaseAddress);
        }

        public override OperationType OperationType => OperationType.Assign;

        public override string OperationAsmString => $"\t{Lhs.AsmString} = ##{Rhs.AsmString}";
    }
}