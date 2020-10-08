using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class SubInstruction: ArithmeticInstruction
    {
        public SubInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }

        public override ArithmeticInstructionType ArithmeticInstructionType =>
            firstOperand.Register.Type == SymbolType.GetType("float")
                ? ArithmeticInstructionType.Float
                : ArithmeticInstructionType.Int;

        public override string InstructionString =>
            ArithmeticInstructionType == ArithmeticInstructionType.Int ? "sub" : "sfsub";
    }
}