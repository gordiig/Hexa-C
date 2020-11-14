using MiniC.Generators;
using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations.AssignOperations
{
    public class RegisterToRegisterOperation: AssignOperation
    {
        public RegisterOperand RhsAsRegisterOperand => rhs as RegisterOperand;

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
        
        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}