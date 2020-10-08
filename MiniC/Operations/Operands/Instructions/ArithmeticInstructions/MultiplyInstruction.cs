using MiniC.Generators;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class MultiplyInstruction: ArithmeticInstruction
    {
        public RegisterOperand SecondOperandAsRegisterOperand => secondOperand as RegisterOperand;
        
        public MultiplyInstruction(RegisterOperand firstOperand, RegisterOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }
        
        public MultiplyInstruction(Register firstOperand, Register secondOperand = null) : 
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
        {
        }

        public override ArithmeticInstructionType ArithmeticInstructionType => 
            firstOperand.Register.Type == SymbolType.GetType("float")
            ? ArithmeticInstructionType.Float
            : ArithmeticInstructionType.Int;
        
        public override string InstructionString =>
            ArithmeticInstructionType == ArithmeticInstructionType.Int ? "mpyi" : "sfmpy";
    }
}