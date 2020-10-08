using MiniC.Generators;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class AddInstruction: ArithmeticInstruction
    {
        public AddInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }

        public AddInstruction(Register firstOperand, IOperand secondOperand = null) :
            base(new RegisterOperand(firstOperand), secondOperand)
        {
        }
        
        public AddInstruction(Register firstOperand, Register secondOperand = null) :
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
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