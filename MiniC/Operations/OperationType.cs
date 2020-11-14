namespace MiniC.Operations
{
    public enum OperationType
    {
        // add, and, not, or, xor, neg, sub
        ALU,
        // call, jump
        J,     
        // r = mem[b,w], deallocframe, dealloc_return
        LD,
        // mem[b,w] = [r,#x], allocframe 
        ST,
        // mpy, all float
        XTYPE,
        // r = [r,#x]
        Assign,
        NonOp,
    }
}