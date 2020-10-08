using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class NegInstruction: ArithmeticInstruction
    {
        public NegInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
        }
        
        public NegInstruction(Register firstOperand, IOperand secondOperand = null) : 
            base(new RegisterOperand(firstOperand), secondOperand)
        {
        }

        public override string InstructionString => "neg";
    }
}