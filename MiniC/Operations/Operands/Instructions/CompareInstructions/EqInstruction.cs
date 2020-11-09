using MiniC.Generators;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public class EqInstruction: CompareInstruction
    {
        protected bool negate;

        public bool Negate => negate;

        public EqInstruction(RegisterOperand firstOperand, IOperand secondOperand, bool negate) :
            base(firstOperand, secondOperand)
        {
            this.negate = negate;
        }

        public EqInstruction(Register firstOperand, IOperand secondOperand, bool negate) :
            base(firstOperand, secondOperand)
        {
            this.negate = negate;
        }
        
        public EqInstruction(Register firstOperand, Register secondOperand, bool negate) :
            base(firstOperand, new RegisterOperand(secondOperand))
        {
            this.negate = negate;
        }

        public override string InstructionString =>
            CompareInstructionType == ArithmeticInstructionType.Int 
                ? $"{(negate ? "!" : "")}cmp.eq" 
                : "sfcmp.eq";
    }
}