namespace MiniC.Generators.RegisterGetters
{
    public interface IRegisterGetter
    {
        Register Zero { get; }
        int Count { get; }

        Register this[int idx] { get; }

        Register GetFreeRegister();
        Register GetFreeRegister(int from);
        void FreeRegister(Register register);
    }
}