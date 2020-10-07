namespace MiniC.Operations.Operands
{
    public class RegisterOperand: IOperand
    {
        private string _register;

        public string Register => _register;

        public RegisterOperand(string register)
        {
            _register = register;
        }

        public OperandType OperandType => OperandType.Register;

        public string AsmString => _register;
    }
}