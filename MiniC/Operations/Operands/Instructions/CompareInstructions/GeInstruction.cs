using MiniC.Generators;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public class GeInstruction: CompareInstruction
    {
        public GeInstruction(RegisterOperand firstOperand, IOperand secondOperand) : 
            base(firstOperand, secondOperand)
        {
        }

        public GeInstruction(Register firstOperand, IOperand secondOperand) : 
            base(firstOperand, secondOperand)
        {
        }
        
        public GeInstruction(Register firstOperand, Register secondOperand) : 
            base(firstOperand, new RegisterOperand(secondOperand))
        {
        }

        public override string InstructionString =>
            $"{(CompareInstructionType == ArithmeticInstructionType.Float ? "sf" : "")}cmp.ge";
    }
}