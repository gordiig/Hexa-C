using MiniC.Exceptions;

namespace MiniC.Operations.Operands.ConstOperands
{
    public class CharConstOperand: ConstOperand
    {
        private char? _constant = null;
        private string _rawConstant;
        private int? _intRepr = null;

        public char? Constant => _constant;
        public string RawConstant => _rawConstant;

        public CharConstOperand(string constantWithApostrophes)
        {
            _rawConstant = constantWithApostrophes;
        }

        public override int IntRepr
        {
            get
            {
                if (_intRepr == null || _constant == null)
                {
                    // Удаляем ''
                    var charValue = _rawConstant.Remove(0, 1);
                    charValue = charValue.Remove(charValue.Length-1);
                
                    var realChar = '\0';
                    if (charValue.Length == 1)
                        realChar = char.Parse(charValue);
                    else if (charValue.Length == 2)
                    {
                        switch (charValue[1])
                        {
                            case 'a':
                                realChar = '\a';
                                break;
                            case 'b':
                                realChar = '\b';
                                break;
                            case '\\':
                                realChar = '\\';
                                break;
                            case '\'':
                                realChar = '\'';
                                break;
                            case 't':
                                realChar = '\t';
                                break;
                            case 'n':
                                realChar = '\n';
                                break;
                            case 'r':
                                realChar = '\r';
                                break;
                            case '0':
                                realChar = '\0';
                                break;
                            default:
                                throw new CodeGenerationException($"Unknown terminator symbol {charValue}");
                        }
                    }
                    else 
                        throw new CodeGenerationException($"Invalid char literal {_rawConstant}");

                    _constant = realChar;
                    _intRepr = (int) realChar;
                }

                return (int) _intRepr;
            }
        }
    }
}