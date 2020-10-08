using MiniC.Operations.Operands;

namespace MiniC.Operations.ConcreteOperations.SectionOperation
{
    public class DataSectionOperation: SectionOperation
    {
        public override SectionType SectionType => SectionType.Data;
    }
}