using MiniC.Generators;

namespace MiniC.Operations.Operands
{
    public class RegisterOperand: IOperand
    {
        private Register _register;

        public Register Register => _register;

        public RegisterOperand(Register register)
        {
            _register = register.Copy();
        }

        public OperandType OperandType => OperandType.Register;

        public string AsmString => _register.ToString();
    }
}