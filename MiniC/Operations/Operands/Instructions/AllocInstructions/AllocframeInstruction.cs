using MiniC.Operations.Operands.ConstOperands;

namespace MiniC.Operations.Operands.Instructions.AllocInstructions
{
    public class AllocframeInstruction: IAsmInstruction
    {
        #region Varialbles and getters

        private IntConstOperand _allocCount;
        
        public int AllocCount => _allocCount.IntRepr;

        #endregion

        #region Constructors

        public AllocframeInstruction(int allocCount)
        {
            _allocCount = new IntConstOperand(allocCount);
        }

        public AllocframeInstruction(IntConstOperand allocCount)
        {
            _allocCount = allocCount;
        }

        #endregion

        #region Interface impl

        public IOperand FirstOperand => _allocCount;
        public IOperand SecondOperand => null;
        public OperandType OperandType => OperandType.Instruction;
        public string InstructionString => "allocframe";

        public string AsmString => $"{InstructionString}({FirstOperand.AsmString})";

        #endregion
    }
}