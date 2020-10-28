using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class MemReadOperation: BaseOperation
    {
        // protected RegisterOperand lhs;
        // protected MemInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public MemInstruction RhsAsMemInstruction => rhs as MemInstruction;
        
        public MemReadOperation(RegisterOperand lhs, MemInstruction rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public MemReadOperation(Register register, MemInstruction rhs)
        {
            lhs = new RegisterOperand(register);
            this.rhs = rhs;
        }
        
        public override OperationType OperationType => OperationType.LD;

        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}