using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;
using MiniC.Operations.Operands.Instructions.CompareInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class CompareOperation: BaseOperation
    {
        // protected RegisterOperand lhs;
        // protected CompareInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public CompareInstruction RhsAsCompareOperation => rhs as CompareInstruction;

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

        public override OperationType OperationType => 
            RhsAsCompareOperation.CompareInstructionType == ArithmeticInstructionType.Int
            ? OperationType.ALU
            : OperationType.XTYPE;

        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}