using MiniC.Generators;
using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class ConditionalOperation: IOperation
    {
        protected RegisterOperand pRegister;
        protected bool negate;
        protected IOperation innerOperation;

        public ConditionalOperation(RegisterOperand pRegister, IOperation innerOperation, bool negate)
        {
            this.pRegister = pRegister;
            this.negate = negate;
            this.innerOperation = innerOperation;
        }
        
        public ConditionalOperation(Register pRegister, IOperation innerOperation, bool negate)
        {
            this.pRegister = new RegisterOperand(pRegister);
            this.negate = negate;
            this.innerOperation = innerOperation;
        }

        public OperationType OperationType => innerOperation.OperationType;
        public IOperand Lhs => innerOperation.Lhs;
        public IOperand Rhs => innerOperation.Rhs;

        public string AsmString => $"if({(negate ? "!" : "")}{pRegister.AsmString}) {innerOperation.AsmString}";
    }
}