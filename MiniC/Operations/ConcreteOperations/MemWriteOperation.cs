using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Operations.Operands.Instructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class MemWriteOperation: BaseOperation
    {
        // protected MemInstruction lhs;
        // protected IOperand rhs;

        public MemInstruction LhsAsMemInstruction => lhs as MemInstruction;

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
        
        public override OperationType OperationType => OperationType.ST;

        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}