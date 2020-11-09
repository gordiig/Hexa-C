using System;
using System.Collections.Generic;
using MiniC.Scopes;

namespace MiniC.Generators
{
    public class Register
    {
        public string Name;
        public bool IsFree = true;
        public SymbolType Type = SymbolType.GetType("int");
        // Если в регистре хранится адрес, то храним его смещение
        private OffsetsSet offsets = new OffsetsSet();

        public OffsetsSet Offsets => offsets;

        public Register(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public static Register SP()
        {
            var register = new Register("SP");
            return register;
        }

        public Register Copy()
        {
            var ans = new Register(Name);
            ans.IsFree = IsFree;
            ans.Type = SymbolType.GetType(Type.Name);
            ans.offsets = new OffsetsSet(offsets);
            return ans;
        }

        public int OffsetsCount => offsets.Count;
        
        public void AddOffset(string offset)
        {
            offsets.Add(offset);
        }
        public void AddRangeOfOffsets(IEnumerable<string> offsets)
        {
            this.offsets.AddRange(offsets);
        }
        public void AddRangeOfOffsets(OffsetsSet offsets)
        {
            this.offsets.AddRange(offsets);
        }

        public void RemoveOffset(string offset)
        {
            offsets.Remove(offset);
        }
        public int RemoveAllOffsets(Predicate<string> p)
        {
            return offsets.RemoveAll(p);
        }
        public void ClearOffsets()
        {
            offsets.Clear();
        }

        public bool ContainsOffset(string offset)
        {
            return offsets.Contains(offset);
        }

        public bool HasEqualOffsets(Register register)
        {
            return offsets.Equals(register.offsets);
        }
        public bool HasEqualOffsets(OffsetsSet offsets)
        {
            return this.offsets.Equals(offsets);
        }
        public bool HasEqualOffsets(IEnumerable<string> offsets)
        {
            return this.offsets.Equals(new OffsetsSet(offsets));
        }
    }
}