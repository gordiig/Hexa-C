using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ConvertInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class ConvertOperation: BaseOperation
    {
        // protected RegisterOperand lhs;
        // protected ConvertInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs as RegisterOperand;
        public ConvertInstruction RhsAsConvertInstruction => rhs as ConvertInstruction;

        public ConvertOperation(RegisterOperand lhs, ConvertInstruction rhs)
        {
            this.lhs = lhs;
            this.rhs = rhs;
        }
        
        public ConvertOperation(Register lhs, ConvertInstruction rhs)
        {
            this.lhs = new RegisterOperand(lhs);
            this.rhs = rhs;
        }
        
        public override OperationType OperationType => OperationType.XTYPE; // TODO

        public override string OperationAsmString => $"\t{Lhs.AsmString};";
    }
}