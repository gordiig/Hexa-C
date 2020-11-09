using MiniC.Generators;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public class GtInstruction: CompareInstruction
    {
        public GtInstruction(RegisterOperand firstOperand, IOperand secondOperand) :
            base(firstOperand, secondOperand)
        {
        }

        public GtInstruction(Register firstOperand, IOperand secondOperand) :
            base(firstOperand, secondOperand)
        {
        }
        
        public GtInstruction(Register firstOperand, Register secondOperand) :
            base(firstOperand, new RegisterOperand(secondOperand))
        {
        }

        public override string InstructionString =>
            $"{(CompareInstructionType == ArithmeticInstructionType.Float ? "sf" : "")}cmp.gt"; 
    }
}