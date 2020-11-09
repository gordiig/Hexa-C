using MiniC.Generators;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class SubInstruction: ArithmeticInstruction
    {
        public SubInstruction(RegisterOperand firstOperand, IOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }
        
        public SubInstruction(Register firstOperand, IOperand secondOperand = null) :
            base(new RegisterOperand(firstOperand), secondOperand)
        {
        }
        
        public SubInstruction(Register firstOperand, Register secondOperand = null) :
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
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