using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class AddInstruction: ArithmeticInstruction
    {
        public AddInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }

        public override ArithmeticInstructionType ArithmeticInstructionType =>
            firstOperand.Register.Type == SymbolType.GetType("float")
                ? ArithmeticInstructionType.Float
                : ArithmeticInstructionType.Int;

        public override string InstructionString =>
            ArithmeticInstructionType == ArithmeticInstructionType.Int ? "add" : "sfadd";
    }
}