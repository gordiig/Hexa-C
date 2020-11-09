using System;
using System.Linq;

namespace MiniC.Generators.RegisterGetters
{
    public class RandomRegisterGetter: BaseRegisterGetter
    {
        public RandomRegisterGetter(string registerPrefix, int registersCount):
            base(registerPrefix, registersCount)
        {
        }

        public override Register GetFreeRegister(int from)
        {
            try
            {
                var register = registers
                    .Skip(from)                      
                    .Where(p => p.IsFree)                        
                    .OrderBy(n => Guid.NewGuid())   
                    .First();
                register.IsFree = false;
                return register;
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("No free registers left");
            }
        }
    }
}