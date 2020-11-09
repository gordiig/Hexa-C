namespace MiniC.Operations.Operands.Instructions
{
    public interface IAsmInstruction: IOperand
    {
        /// <summary>
        /// Первый операнд инструкции
        /// </summary>
        IOperand FirstOperand { get; }
        
        /// <summary>
        /// Второй операнд иснтрукции
        /// </summary>
        IOperand SecondOperand { get; }
        
        /// <summary>
        /// Имя инструкции
        /// </summary>
        string InstructionString { get; }
    }
}