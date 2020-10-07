namespace MiniC.Operations.Operands.ConstOperands
{
    public abstract class ConstOperand: IOperand
    {
        #region Own methods

        public virtual int IntRepr { get; }

        #endregion
        
        #region Interface impl

        public OperandType OperandType => OperandType.Constant;

        public string AsmString => $"#{IntRepr}";

        #endregion

    }
}