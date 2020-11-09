using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public class LtInstruction: GtInstruction
    {
        public LtInstruction(RegisterOperand firstOperand, IOperand secondOperand) :
         base(firstOperand, secondOperand)
        {
        }

        public LtInstruction(Register firstOperand, IOperand secondOperand) :
         base(firstOperand, secondOperand)
        {
        }

        public LtInstruction(Register firstOperand, Register secondOperand) :
         base(firstOperand, secondOperand)
        {
        }
        
        // LT делается через GT простым свапом параметров
        public override string AsmString => $"{InstructionString}({SecondOperand.AsmString}, {FirstOperand.AsmString})";
    }
}