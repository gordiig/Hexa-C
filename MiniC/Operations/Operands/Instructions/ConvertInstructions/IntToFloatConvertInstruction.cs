using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ConvertInstructions
{
    public class IntToFloatConvertInstruction: ConvertInstruction
    {
        public IntToFloatConvertInstruction(RegisterOperand reg) : base(reg)
        {
        }

        public IntToFloatConvertInstruction(Register reg) : base(reg)
        {
        }

        public override string InstructionString => "convert_w2sf";
    }
}