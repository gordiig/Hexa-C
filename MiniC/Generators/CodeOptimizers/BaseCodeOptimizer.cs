using System.Collections.Generic;
using System.Linq;
using MiniC.Generators.ListingScopeTreeDir;
using MiniC.Generators.Utils;
using MiniC.Operations;
using MiniC.Operations.ConcreteOperations.LDOperations;
using MiniC.Operations.ConcreteOperations.STOperations;
using MiniC.Operations.Operands;

namespace MiniC.Generators.CodeOptimizers
{
    public abstract class BaseCodeOptimizer: ICodeOptimizer
    {
        // Множество регистров, в которые записывали один и несколько раз соответственно
        private HashSet<Register> writeOnceRegisters = new HashSet<Register>();
        private HashSet<Register> writeMultipleRegisters = new HashSet<Register>();
        private HashSet<OffsetsSet> writeOnceOffsets = new HashSet<OffsetsSet>();
        
        public abstract ListingScopeTree Optimize(ListingScopeTree scopeTree);

        protected bool CheckIfRegisterWasWrittenMultipleTimes(Register register, bool trueIfOnce = false, 
            bool commitWriting = true)
        {
            // SP игнорируем
            if (register.Equals(Register.SP()))
                return false;
            
            // Если была запись несколько раз
            if (writeMultipleRegisters.Contains(register))
                return true;
            
            // Если была запись один раз
            if (writeOnceRegisters.Contains(register))
            {
                // Фиксируем если нужно 
                if (commitWriting)
                {
                    writeOnceRegisters.Remove(register);
                    writeMultipleRegisters.Add(register);   
                }
                // Возвращаем то, что просили если запись только один раз была
                return trueIfOnce;
            }

            // Фиксируем единичную запись если нужно
            if (commitWriting)
                writeOnceRegisters.Add(register);

            return false;
        }

        protected bool CheckIfOffsetWasWrittenOnce(OffsetsSet offsetsSet, bool commitChanges = true)
        {
            // Если была запись по адресу
            if (writeOnceOffsets.Contains(offsetsSet)) 
                return true;
            
            // Фиксируем, если надо
            if (commitChanges) 
                writeOnceOffsets.Add(offsetsSet);
            
            return false;
        }
        
        protected bool CheckLhs(IOperation operation)
        {
            switch (operation)
            {
                // Если операция -- запись в память, то проверяем адрес
                case MemWriteOperation memWriteOperation:
                    var lhsOffsets = memWriteOperation.LhsAsMemInstruction.OffsetsSet;
                    return !CheckIfOffsetWasWrittenOnce(lhsOffsets);
                // Иначе
                default:
                    // Если лхс -- регистр, то проверяем чтобы он не записывался ни разу
                    if (operation.Lhs is RegisterOperand registerOperand)
                        return !CheckIfRegisterWasWrittenMultipleTimes(registerOperand.Register, true);
                    // Если не регистр, то все ок
                    return true;
            }
        }

        protected bool CheckRegisterForRhs(Register register, BlockScope block)
        {
            if (register == null)
                return true;
            
            // Если записей было несколько, то false
            if (CheckIfRegisterWasWrittenMultipleTimes(register, false, false))
                return false;
            // Если запись была один раз
            if (CheckIfRegisterWasWrittenMultipleTimes(register, true, false))
                // Если в блоке слева нет этого регистра, то false
                if (!block.LhsRegisters.Contains(register))
                    return false;

            return true;
        }
        
        protected bool CheckRhs(IOperation operation, BlockScope block)
        {
            // Если операция -- чтение из памяти, то проверяем, что адреса чтения нет в адресах, в которые происходила запись
            if (operation is MemReadOperation memReadOperation)
                if (writeOnceOffsets.Contains(memReadOperation.RhsAsMemInstruction.OffsetsSet))
                    return false;
            
            // Проверка регистров
            var operationRegisters = new RegisterOperandsFromOperation(operation);
            if (!CheckRegisterForRhs(operationRegisters.Rhs1?.Register, block))
                return false;
            if (!CheckRegisterForRhs(operationRegisters.Rhs2?.Register, block))
                return false;

            return true;
        }

        protected ListingScopeTree OptimizeJob(List<IOperation> operations, string scopeName, ListingScopeType scopeType)
        {
            // Возвращаемый блок
            var result = new ListingScopeTree(scopeName, scopeType);
            
            while (operations.Count > 0)
            {
                // Блок
                var block = new BlockScope();
                // Индексы элементов, которые необходимо убрать из рассматриваемых операций для слудующей итерации
                // верхнего цикла оптимизации
                var idxsToDelete = new List<int>();
                // Индекс рассматриваемой инструкции
                int i = 0;
                while (i < operations.Count && !block.IsFull)
                {
                    // Проверка левого и правого операндов
                    if (CheckLhs(operations[i]) && CheckRhs(operations[i], block))
                    {
                        // Попытка добавить операцию в блок
                        if (!block.AddToBlock(operations[i]))
                        {
                            // Если в блок не удалось добавить операцию, и при этом блок пустой,
                            // то добавляем эту операцию в результат, и запоминаем индекс для удаления
                            if (block.IsEmpty)
                            {
                                result.Add(operations[i]);
                                idxsToDelete.Add(i);
                            }
                        }
                        else
                            // Иначе, если в блок добавилась инструкция, то просто добавляем индекс на удаление    
                            idxsToDelete.Add(i);
                    }
                    i++;
                }

                // Убираем из рассматриваемых операций те, что мы добавили в блок
                for (int deleteIdx = 0; deleteIdx < idxsToDelete.Count; deleteIdx++)
                    // Вычитание, потому что после удаления элемента список становится меньше на 1 элемент,
                    // а индексы у нас идут по возрастанию
                    operations.RemoveAt(idxsToDelete[deleteIdx] - deleteIdx); 
                // Чистим множества регистров, в которые была запись
                writeOnceRegisters.Clear();
                writeMultipleRegisters.Clear();
                writeOnceOffsets.Clear();
                // Добавляем параллельный блок в результрущий скоуп
                result.Add(block);
            }

            return result;
        }
    }
}