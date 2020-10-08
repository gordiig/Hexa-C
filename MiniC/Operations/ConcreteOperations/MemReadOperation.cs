using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class MemReadOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected MemInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs;
        public MemInstruction RhsAsMemInstruction => rhs;
        
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

        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;
        public OperationType OperationType => OperationType.LD;

        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}