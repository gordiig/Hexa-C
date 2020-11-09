using MiniC.Generators;

namespace MiniC.Operations.Operands.Instructions.ConvertInstructions
{
    public class FloatToIntConvertInstruction: ConvertInstruction
    {
        public FloatToIntConvertInstruction(RegisterOperand reg) : base(reg)
        {
        }

        public FloatToIntConvertInstruction(Register reg) : base(reg)
        {
        }

        public override string InstructionString => "convert_sf2w";
    }
}