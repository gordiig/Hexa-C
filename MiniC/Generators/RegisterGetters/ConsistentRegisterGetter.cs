using System;

namespace MiniC.Generators.RegisterGetters
{
    public class ConsistentRegisterGetter: BaseRegisterGetter
    {
        public ConsistentRegisterGetter(string registerPrefix, int registersCount):
            base(registerPrefix, registersCount)
        {
        }

        public override Register GetFreeRegister(int from)
        {
            for (int i = from; i < registers.Length; i++)
            {
                if (!registers[i].IsFree) 
                    continue;
                registers[i].IsFree = false;
                return registers[i];
            }

            throw new ArgumentException("No free registers left");
        }
        
    }
}