using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public class LeInstruction: GeInstruction
    {
        public LeInstruction(RegisterOperand firstOperand, IOperand secondOperand) :
            base(firstOperand, secondOperand)
        {
        }

        public LeInstruction(Register firstOperand, IOperand secondOperand) : 
            base(firstOperand, secondOperand)
        {
        }

        public LeInstruction(Register firstOperand, Register secondOperand) : 
            base(firstOperand, secondOperand)
        {
        }

        // LE делается через GE простым свапом параметров
        public override string AsmString => $"{InstructionString}({SecondOperand.AsmString}, {FirstOperand.AsmString})";
    }
}