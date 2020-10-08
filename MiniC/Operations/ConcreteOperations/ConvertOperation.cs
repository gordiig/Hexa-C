using MiniC.Generators;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions.ConvertInstructions;

namespace MiniC.Operations.ConcreteOperations
{
    public class ConvertOperation: IOperation
    {
        protected RegisterOperand lhs;
        protected ConvertInstruction rhs;

        public RegisterOperand LhsAsRegisterOperand => lhs;
        public ConvertInstruction RhsAsConvertInstruction => rhs;

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
        
        public IOperand Lhs => lhs;
        public IOperand Rhs => rhs;
        public OperationType OperationType => OperationType.XTYPE; // TODO

        public string AsmString => $"\t{Lhs.AsmString};";
    }
}