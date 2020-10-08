using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;
using MiniC.Operations.Operands.Instructions.CompareInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class CompareOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected CompareInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs;
        public CompareInstruction RhsAsCompareOperation => rhs;

        public CompareOperation(RegisterOperand lhs, CompareInstruction rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }

        public CompareOperation(Register lhs, CompareInstruction rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = rhs;
        }

        public OperationType OperationType => rhs.CompareInstructionType == ArithmeticInstructionType.Int
            ? OperationType.ALU
            : OperationType.XTYPE;  
        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;

        public string AsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}