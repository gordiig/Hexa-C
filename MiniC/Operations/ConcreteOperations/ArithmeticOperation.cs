using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class ArithmeticOperation: BaseOperation
    {
        // protected ArithmeticInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public ArithmeticInstruction RhsAsArithmeticInstruction => rhs as ArithmeticInstruction;

        public ArithmeticOperation(Register lhs, ArithmeticInstruction rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = rhs;
        }

        public override OperationType OperationType => 
            RhsAsArithmeticInstruction.ArithmeticInstructionType == ArithmeticInstructionType.Int
            ? OperationType.ALU
            : OperationType.XTYPE;

        public override string OperationAsmString => $"\t{Lhs.AsmString} = {Rhs.AsmString};";
    }
}