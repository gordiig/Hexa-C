namespace MiniC.Operations
{
    public interface IAsmWritable
    {
        /// <summary>
        /// Строка, которую нужно записать в выходной файл
        /// </summary>
        string AsmString { get; }
    }
}