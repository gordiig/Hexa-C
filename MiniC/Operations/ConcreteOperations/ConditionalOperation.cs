using MiniC.Generators;
using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations
{
    public class ConditionalOperation: BaseOperation
    {
        protected RegisterOperand pRegister;
        protected bool negate;
        protected IOperation innerOperation;

        public RegisterOperand PRegister => pRegister;
        public bool Negate => negate;
        public IOperation InnerOperation => innerOperation;

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

        public override OperationType OperationType => innerOperation.OperationType;

        public override string OperationAsmString => 
            $"if({(negate ? "!" : "")}{pRegister.AsmString}) {innerOperation.AsmString}";
    }
}