namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class NegInstruction: ArithmeticInstruction
    {
        public NegInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) : 
            base(firstOperand, secondOperand)
        {
        }

        public override string InstructionString => "neg";
    }
}