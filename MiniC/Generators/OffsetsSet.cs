using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniC.Generators
{
    public class OffsetsSet: ICloneable
    {
        private List<string> offsets;
        
        public OffsetsSet()
        {
            offsets = new List<string>();
        }
        public OffsetsSet(string offset)
        {
            offsets = new List<string>();
            offsets.Add(offset);
        }
        public OffsetsSet(IEnumerable<string> offsets)
        {
            this.offsets = new List<string>(offsets);
        }
        public OffsetsSet(OffsetsSet copy)
        {
            offsets = new List<string>(copy.offsets);
        }

        public object Clone()
        {
            return new OffsetsSet(this);
        }

        public int Count => offsets.Count;

        public void Add(string offset)
        {
            offsets.Add(offset);
        }
        public void AddRange(IEnumerable<string> offsets)
        {
            this.offsets.AddRange(offsets);
        }
        public void AddRange(OffsetsSet offsets)
        {
            this.offsets.AddRange(offsets.offsets);
        }
        
        public void Remove(string offset)
        {
            offsets.Remove(offset);
        }
        public int RemoveAll(Predicate<string> p)
        {
            return offsets.RemoveAll(p);
        }
        public void Clear()
        {
            offsets.Clear();
        }

        public bool Contains(string offset)
        {
            return offsets.Contains(offset);
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is OffsetsSet))
                return false;
            var set = new OffsetsSet((OffsetsSet) obj);
            if (set.Count != Count)
                return false;
            
            var me = new List<string>(offsets);
            while (me.Count > 0)
            {
                var el = me.First();
                
                var elCountInMe = me.Count(s => s == el);
                var elCountInObj = set.offsets.Count(s => s == el);
                if (elCountInMe != elCountInObj)
                    return false;
                
                me.RemoveAll(s => s == el);
                set.RemoveAll(s => s == el);
            }

            return set.Count == 0;
        }
    }
}