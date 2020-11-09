namespace MiniC.Operations.Operands.Instructions.JumpInstructions
{
    public abstract class BaseJumpInstruction: IAsmInstruction
    {
        #region Variables and basic getters

        private LabelOperand _label;

        public string Label => _label.AsmString;

        #endregion

        #region Constructors

        public BaseJumpInstruction(string label)
        {
            _label = new LabelOperand(label);
        }
        
        public BaseJumpInstruction(LabelOperand label)
        {
            _label = label;
        }

        #endregion

        #region Interface impl

        public IOperand FirstOperand => _label;
        public IOperand SecondOperand => null;
        public OperandType OperandType => OperandType.Instruction;
        public virtual string InstructionString { get; }

        public string AsmString => $"{InstructionString} {FirstOperand.AsmString}";

        #endregion
    }
}