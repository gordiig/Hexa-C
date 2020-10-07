using System;
using System.Linq;

namespace MiniC.Operations.Operands.ConstOperands
{
    public class IntConstOperand: ConstOperand
    {
        private int _constant;

        public IntConstOperand(int constant)
        {
            _constant = constant;
        }

        public IntConstOperand(string intStr)
        {
            if (intStr.ToLower().Contains("0x"))
                _constant = Convert.ToInt32(intStr, 16);
            else if (intStr.First() == '0')
                _constant = Convert.ToInt32(intStr, 8);
            else
                _constant = Convert.ToInt32(intStr, 10);
        }

        public override int IntRepr => _constant;
    }
}