using MiniC.Operations;
using MiniC.Operations.ConcreteOperations;
using MiniC.Operations.ConcreteOperations.AssignOperations;
using MiniC.Operations.ConcreteOperations.LDOperations;
using MiniC.Operations.ConcreteOperations.STOperations;
using MiniC.Operations.Operands;

namespace MiniC.Generators.Utils
{
    public class RegisterOperandsFromOperation
    {
        private RegisterOperand lhs = null;
        private RegisterOperand rhs1 = null;
        private RegisterOperand rhs2 = null;
        private RegisterOperand conditional = null;

        public RegisterOperand Lhs => lhs;
        public RegisterOperand Rhs1 => rhs1;
        public RegisterOperand Rhs2 => rhs2;
        public RegisterOperand Conditional => conditional;

        public RegisterOperandsFromOperation(IOperation operation)
        {
            Init(operation);
        }

        protected void Init(IOperation operation)
        {
            switch (operation)
            {
                case AssignOperation assignOperation:
                    InitForAssignOperation(assignOperation);
                    break;
                
                case MemWriteOperation memWriteOperation:
                    InitForMemWriteOperation(memWriteOperation);
                    break;
                
                case MemReadOperation memReadOperation:
                    InitForMemReadOperation(memReadOperation);
                    break;
                    
                case ArithmeticOperation arithmeticOperation:
                    InitForArithmeticOperation(arithmeticOperation);
                    break;
                
                case CompareOperation compareOperation:
                    InitForCompareOperation(compareOperation);
                    break;
                
                case ConditionalOperation conditionalOperation:
                    InitForConditionalOperation(conditionalOperation);
                    break;
            }
        }
        
        protected void InitForAssignOperation(AssignOperation assignOperation)
        {
            lhs = assignOperation.LhsAsRegisterOperand;
            if (assignOperation.Rhs is RegisterOperand registerOperand)
                rhs1 = registerOperand;
        }

        protected void InitForMemWriteOperation(MemWriteOperation memWriteOperation)
        {
            lhs = memWriteOperation.LhsAsMemInstruction.FirstOperandAsRegisterOperand;
            if (memWriteOperation.Rhs is RegisterOperand registerOperand)
                rhs1 = registerOperand;
        }

        protected void InitForMemReadOperation(MemReadOperation memReadOperation)
        {
            lhs = memReadOperation.LhsAsRegisterOperand;
            rhs1 = memReadOperation.RhsAsMemInstruction.FirstOperandAsRegisterOperand;
        }
        
        protected void InitForArithmeticOperation(ArithmeticOperation arithmeticOperation)
        {
            lhs = arithmeticOperation.LhsAsRegisterOperand;
            rhs1 = arithmeticOperation.RhsAsArithmeticInstruction.FirstOperandAsRegisterOperand;
            if (arithmeticOperation.RhsAsArithmeticInstruction.SecondOperand is RegisterOperand registerOperand)
                rhs2 = registerOperand;
        }

        protected void InitForCompareOperation(CompareOperation compareOperation)
        {
            lhs = compareOperation.LhsAsRegisterOperand;
            rhs1 = compareOperation.RhsAsCompareOperation.FirstOperandAsRegisterOperand;
            if (compareOperation.RhsAsCompareOperation.SecondOperand is RegisterOperand registerOperand)
                rhs2 = registerOperand;
        }

        protected void InitForConditionalOperation(ConditionalOperation conditionalOperation)
        {
            conditional = conditionalOperation.PRegister;
            Init(conditionalOperation.InnerOperation);
        }
        
    }
}