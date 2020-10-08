using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ArithmeticInstructions
{
    public class OrInstruction: ArithmeticInstruction
    {
        public OrInstruction(RegisterOperand firstOperand, RegisterOperand secondOperand = null) :
            base(firstOperand, secondOperand)
        {
        }
        
        public OrInstruction(Register firstOperand, Register secondOperand = null) :
            base(new RegisterOperand(firstOperand), new RegisterOperand(secondOperand))
        {
        }

        public RegisterOperand SecondOperandAsRegisterOperand => secondOperand as RegisterOperand;
        
        public override string InstructionString => "or";
    }
}