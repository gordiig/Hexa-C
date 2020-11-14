using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Scopes;

namespace MiniC.Operations.ConcreteOperations.AssignOperations
{
    public class GlobalVariableAddressToRegisterOperation: AssignOperation
    {
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

        public override string OperationAsmString => $"\t{Lhs.AsmString} = ##{Rhs.AsmString}";
    }
}