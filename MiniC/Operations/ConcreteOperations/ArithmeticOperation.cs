using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class ArithmeticOperation: IOperation
    {
        protected IOperand lhs;
        protected ArithmeticInstruction rhs;

        public ArithmeticInstruction RhsAsArithmeticInstruction => rhs;

        public ArithmeticOperation(IOperand lhs, ArithmeticInstruction rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;

        public OperationType OperationType => rhs.ArithmeticInstructionType == ArithmeticInstructionType.Int
            ? OperationType.ALU
            : OperationType.XTYPE;

        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}