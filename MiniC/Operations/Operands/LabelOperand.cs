namespace MiniC.Operations.Operands
{
    public class LabelOperand: IOperand
    {
        private string _label;

        public LabelOperand(string label)
        {
            _label = label;
        }

        public OperandType OperandType => OperandType.Label;
        public string AsmString => _label;
    }
}