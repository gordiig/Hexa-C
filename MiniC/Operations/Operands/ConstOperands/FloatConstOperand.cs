using System;

namespace MiniC.Operations.Operands.ConstOperands
{
    public class FloatConstOperand: ConstOperand
    {
        private float _constant;
        private int? _intRepr = null; 

        public float Constant => _constant;
        
        public FloatConstOperand(float constant)
        {
            _constant = constant;
        }
        
        public FloatConstOperand(string constant)
        {
            _constant = float.Parse(constant);
        }

        public override OperandType OperandType => OperandType.Constant_f;

        public override int IntRepr
        {
            get
            {
                if (_intRepr == null)
                {
                    var floatHex = BitConverter.GetBytes(_constant);
                    _intRepr = BitConverter.ToInt32(floatHex, 0);  
                } 
                return (int) _intRepr;
            }
        }
    }
}