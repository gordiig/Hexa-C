using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Operations.Operands.Instructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class MemWriteOperation: IOperation
    {
        protected MemInstruction lhs;
        protected IOperand rhs;

        public MemInstruction LhsAsMemInstruction => lhs;

        public MemWriteOperation(MemInstruction lhs, RegisterOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public MemWriteOperation(MemInstruction lhs, ConstOperand rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }
        
        public MemWriteOperation(MemInstruction lhs, int rhs)
        {
            this.lhs = lhs;
            this.rhs = new IntConstOperand(rhs);
        }

        public MemWriteOperation(MemInstruction lhs, Register register)
        {
            this.lhs = lhs;
            rhs = new RegisterOperand(register);
        }

        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;
        public OperationType OperationType => OperationType.ST;

        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}