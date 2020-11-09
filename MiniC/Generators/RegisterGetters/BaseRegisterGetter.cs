using System.Collections;

namespace MiniC.Generators.RegisterGetters
{
    public abstract class BaseRegisterGetter: IRegisterGetter
    {
        protected Register[] registers;

        protected BaseRegisterGetter(string registerPrefix, int registersCount)
        {
            registers = new Register[registersCount];
            for (int i = 0; i < registersCount; i++)
            {
                registers[i] = new Register($"{registerPrefix}{i}");
            }
        }

        public Register Zero => registers[0];
        public int Count => registers.Length;
        public Register this[int idx] => registers[idx];

        public abstract Register GetFreeRegister(int from);

        public virtual Register GetFreeRegister()
        {
            return GetFreeRegister(0);
        }

        public virtual void FreeRegister(Register register)
        {
            register.ClearOffsets();
            register.IsFree = true;
        }
    }
}