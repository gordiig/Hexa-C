namespace MiniC.Operations.Operands
{
    public interface IOperand: IAsmWritable
    {
        /// <summary>
        /// Тип операнда
        /// </summary>
        OperandType OperandType { get; }

        string ToString() => AsmString;
    }
}