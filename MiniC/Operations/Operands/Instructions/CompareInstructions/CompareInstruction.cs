using MiniC.Generators;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;
using MiniC.Scopes;

namespace MiniC.Operations.Operands.Instructions.CompareInstructions
{
    public abstract class CompareInstruction: IAsmInstruction
    {
        protected RegisterOperand firstOperand;
        protected IOperand secondOperand;

        public RegisterOperand FirstOperandAsRegisterOperand => firstOperand;

        protected CompareInstruction(RegisterOperand firstOperand, IOperand secondOperand)
        {
            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;
        }
        
        protected CompareInstruction(Register firstOperand, IOperand secondOperand)
        {
            this.firstOperand = new RegisterOperand(firstOperand);
            this.secondOperand = secondOperand;
        }

        public ArithmeticInstructionType CompareInstructionType =>
            firstOperand.Register.Type == SymbolType.GetType("float")
                ? ArithmeticInstructionType.Float
                : ArithmeticInstructionType.Int;

        public OperandType OperandType => OperandType.Instruction;
        public IOperand FirstOperand => firstOperand;
        public IOperand SecondOperand => secondOperand;
        public abstract string InstructionString { get; }

        public virtual string AsmString => $"{InstructionString}({FirstOperand.AsmString}, {SecondOperand.AsmString})";
    }
}