using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniC.Generators.Utils;
using MiniC.Operations;
using MiniC.Operations.ConcreteOperations;
using MiniC.Operations.ConcreteOperations.AssignOperations;
using MiniC.Operations.ConcreteOperations.LDOperations;
using MiniC.Operations.ConcreteOperations.STOperations;
using MiniC.Operations.Operands;
using MiniC.Operations.Operands.Instructions;

namespace MiniC.Generators.ListingScopeTreeDir
{
    public class BlockScope: ListingScopeTree
    {
        private Dictionary<Register, int> lhsRegisters = new Dictionary<Register, int>();
        private Dictionary<OffsetsSet, int> lhsAddresses = new Dictionary<OffsetsSet, int>();
        
        public BlockScope(string name = "block", ListingScopeType type = ListingScopeType.ConcurrentBlock)
            : base(name, type)
        {
            scopeEntries = new ArrayList(4);
            for (int i = 0; i < 4; i++)
                scopeEntries.Add(null);
        }

        public IEnumerable<Register> LhsRegisters => lhsRegisters.Keys;
        public IEnumerable<OffsetsSet> LhsAddresses => lhsAddresses.Keys;

        // LD, ST, ALU32, ASSIGN
        public IOperation First
        {
            get => (IOperation) scopeEntries[0];
            set => scopeEntries[0] = value;
        }
        // LD, ST, ALU32, ASSIGN
        public IOperation Second
        {
            get => (IOperation) scopeEntries[1];
            set => scopeEntries[1] = value;
        }
        // XTYPE, ALU32, J, ASSIGN
        public IOperation Third
        {
            get => (IOperation) scopeEntries[2];
            set => scopeEntries[2] = value;
        }
        // XTYPE, ALU32, J, ASSIGN
        public IOperation Fourth
        {
            get => (IOperation) scopeEntries[3];
            set => scopeEntries[3] = value;
        }

        public bool IsFull
        {
            get
            {
                foreach (var scopeEntry in scopeEntries)
                    if (scopeEntry == null)
                        return false;
                return true;
            }
            
        }

        public bool IsEmpty
        {
            get
            {
                foreach (var scopeEntry in scopeEntries)
                    if (scopeEntry != null)
                        return false;
                return true;
            }
        }

        protected bool IsRegisterInCurrentLhsRegisters(Register register)
        {
            return lhsRegisters.ContainsKey(register);
        }

        protected bool IsAddressInCurrentLhsAddresses(OffsetsSet offsetsSet)
        {
            return lhsAddresses.ContainsKey(offsetsSet);
        }

        protected bool DoesRhsRegisterNeedNew(Register register, int positionToAddRhs)
        {
            // Если такого регистра нет в лхс, то .new не нужен
            if (!lhsRegisters.TryGetValue(register, out int idx)) 
                return false;
            
            // Если он есть, но он не SP, то для подстановки .new нужно, чтобы индекс из lhs был меньше, чем текущий
            if (!register.Equals(Register.SP())) 
                return idx < positionToAddRhs;
            
            // Если регистр -- SP, то .new нужен, если до текущего индекса были операции аллокации или деаллокации
            for (int i = 0; i < positionToAddRhs; i++)
                if (scopeEntries[i] is AllocOperation || scopeEntries[i] is DeallocOperation)
                    return true;
            
            return false;
        }

        protected void AddToEntries(IOperation operation, int position)
        {
            // Добавляем в ентрис
            scopeEntries[position] = operation;
            
            // Добавляем регистр или адрес слева в словарики
            if (operation.Lhs is RegisterOperand registerOperand)
                lhsRegisters[registerOperand.Register] = position;
            else if (operation.Lhs is MemInstruction memInstruction)
                lhsAddresses[memInstruction.OffsetsSet] = position;
            
            // Проходимся по всем операциям и устанавливаем .new где нужно
            for (int i = 0; i < 4; i++)
            {
                var scopeEntryRegisters = new RegisterOperandsFromOperation((IOperation) scopeEntries[i]);
                if (scopeEntryRegisters.Rhs1 != null)
                    scopeEntryRegisters.Rhs1.NeedsNew = DoesRhsRegisterNeedNew(scopeEntryRegisters.Rhs1.Register, i);
                if (scopeEntryRegisters.Rhs2 != null)
                    scopeEntryRegisters.Rhs2.NeedsNew = DoesRhsRegisterNeedNew(scopeEntryRegisters.Rhs2.Register, i);
                if (scopeEntryRegisters.Conditional != null)
                    scopeEntryRegisters.Conditional.NeedsNew = 
                        DoesRhsRegisterNeedNew(scopeEntryRegisters.Conditional.Register, i);
            }
        }
        
        protected int FindPlaceForOperation(IOperation op)
        {
            // Устанавливаем начальные и конечные индексы для цикла
            int startIdx, endIdx;
            switch (op.OperationType)
            {
                case OperationType.Assign: case OperationType.ALU:
                    startIdx = 0;
                    endIdx = 4;
                    break;
                case OperationType.LD: case OperationType.ST:
                    startIdx = 0;
                    endIdx = 2;
                    break;
                case OperationType.J: case OperationType.XTYPE:
                    startIdx = 2;
                    endIdx = 4;
                    break;
                default:
                    return -1;
            }
            
            // Ищем подходящее место
            for (int i = startIdx; i < endIdx; i++)
                if (scopeEntries[i] == null)
                    return i;
            return -1;
        }
        
        public bool AddToBlock(IOperation op)
        {
            // Если места для операции нет, возвращаем фолс 
            int freePlace = FindPlaceForOperation(op);
            if (freePlace == -1)
                return false;

            return op.OperationType switch
            {
                OperationType.Assign => AddAssign((AssignOperation) op, freePlace),
                OperationType.ST => AddStore((STOperation) op, freePlace),
                OperationType.LD => AddLoad((LDOperation) op, freePlace),
                OperationType.ALU => AddAlu(op, freePlace),
                OperationType.XTYPE => AddXtype(op, freePlace),
                _ => false
            };
        }

        protected bool AddAssign(AssignOperation op, int positionToAdd)
        {
            // Проверяем, чтобы лхс из операции не был таким же, как лхс в уже имеющихся операциях
            var lhs = op.LhsAsRegisterOperand;
            if (IsRegisterInCurrentLhsRegisters(lhs.Register))
                return false;
            // Вставка
            AddToEntries(op, positionToAdd);
            return true;
        }

        protected bool AddStore(STOperation op, int positionToAdd)
        {
            // Если операция -- аллок, то не вставляем ее в блок
            if (op is AllocOperation)
                return false;
            // Преобразуем к мем райт
            var memWriteOperation = (MemWriteOperation) op;
            var lhs = memWriteOperation.LhsAsMemInstruction;
            // Проверяем, чтобы лхс из операции не был таким же, как лхс в уже имеющихся операциях
            if (IsAddressInCurrentLhsAddresses(lhs.OffsetsSet))
                return false;
            // Вставка
            AddToEntries(op, positionToAdd);
            return true;
        }

        protected bool AddLoad(LDOperation op, int positionToAdd)
        {
            // Если операция -- деаллок, то не вставляем ее в блок
            if (op is DeallocOperation)
                return false;
            // Преобразуем к мем рид
            var memReadOperation = (MemReadOperation) op;
            var lhs = memReadOperation.LhsAsRegisterOperand;
            var rhs = memReadOperation.RhsAsMemInstruction;
            // Проверяем, чтобы лхс из операции не был таким же, как лхс в уже имеющихся операциях
            if (IsRegisterInCurrentLhsRegisters(lhs.Register))
                return false;
            // Проверяем, чтобы адрес рхс из операции не был таким же, как имеющиеся адреса в лхс
            if (IsAddressInCurrentLhsAddresses(rhs.OffsetsSet))
                return false;
            // Вставляем операцию
            AddToEntries(op, positionToAdd);
            return true;
        }

        protected bool AddAlu(IOperation op, int positionToAdd)
        {
            switch (op)
            {
                // Если ноп, то просто добавляем и все
                case NopOperation nopOperation:
                    AddToEntries(nopOperation, positionToAdd);
                    return true;
                // Если это арифметическая операция
                case ArithmeticOperation arithmeticOperation:
                    return AddArithmetic(arithmeticOperation, positionToAdd);
                // Если это операция сравнения
                case CompareOperation compareOperation:
                    return AddCompare(compareOperation, positionToAdd);
                // Иначе
                default:
                    throw new InvalidOperationException();
            }
        }

        protected bool AddXtype(IOperation op, int positionToAdd)
        {
            return op switch
            {
                // Если это операция преобразования типов
                ConvertOperation convertOperation => AddConvert(convertOperation, positionToAdd),
                // Если это арифметическая операция
                ArithmeticOperation arithmeticOperation => AddArithmetic(arithmeticOperation, positionToAdd),
                // Если это операция сравнения
                CompareOperation compareOperation => AddCompare(compareOperation, positionToAdd),
                _ => throw new InvalidOperationException()
            };
        }

        protected bool AddArithmetic(ArithmeticOperation op, int positionToAdd)
        {
            // Проверяем, есть ли лхс в текущих регистрах лхс
            var lhsRegister = op.LhsAsRegisterOperand.Register;
            if (IsRegisterInCurrentLhsRegisters(lhsRegister))
                return false;
            // Записывам операцию
            AddToEntries(op, positionToAdd);
            return true;
        }

        protected bool AddCompare(CompareOperation op, int positionToAdd)
        {
            // Проверяем, есть ли лхс в текущих регистрах лхс
            var lhsRegister = op.LhsAsRegisterOperand.Register;
            if (IsRegisterInCurrentLhsRegisters(lhsRegister))
                return false;
            // Записывам операцию
            AddToEntries(op, positionToAdd);
            return true;
        }

        protected bool AddConvert(ConvertOperation op, int positionToAdd)
        {
            // Если лхс регистр есть в текущих лхс-регистрах, то нельзя вставить
            var lhsRegister = op.LhsAsRegisterOperand.Register;
            if (IsRegisterInCurrentLhsRegisters(lhsRegister))
                return false;
            // Вставляем в блок
            AddToEntries(op, positionToAdd);
            return true;
        }
        
        public override string AsmString
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("\t{");
                foreach (var entry in scopeEntries)
                {
                    if (entry == null) 
                        continue;
                    stringBuilder.Append("\t");
                    stringBuilder.AppendLine(((IAsmWritable) entry)?.AsmString);
                }
                stringBuilder.AppendLine("\t}");
                return stringBuilder.ToString();
            }
        }
    }
}