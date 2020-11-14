using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ConvertInstructions
{
    public abstract class ConvertInstruction: IAsmInstruction
    {
        protected RegisterOperand reg;

        public RegisterOperand FirstOperandAsRegisterOperand => reg;
        public bool NeedsNew
        {
            get => reg.NeedsNew;
            set => reg.NeedsNew = value;
        }

        public ConvertInstruction(RegisterOperand reg)
        {
            this.reg = reg;
        }

        public ConvertInstruction(Register reg)
        {
            this.reg = new RegisterOperand(reg);
        }

        public OperandType OperandType => OperandType.Instruction;
        public IOperand FirstOperand => reg;
        public IOperand SecondOperand => null;
        public abstract string InstructionString { get; }

        public string AsmString => $"{InstructionString}({FirstOperand.AsmString})";
    }
}