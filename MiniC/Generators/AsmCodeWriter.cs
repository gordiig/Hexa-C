using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using MiniC.Exceptions;
using MiniC.Generators.ListingScopeTreeDir;
using MiniC.Generators.RegisterGetters;
using MiniC.Operations;
using MiniC.Operations.ConcreteOperations;
using MiniC.Operations.ConcreteOperations.AddGlobalSymbolOperations;
using MiniC.Operations.ConcreteOperations.SectionOperation;
using MiniC.Operations.Operands.ConstOperands;
using MiniC.Operations.Operands.Instructions;
using MiniC.Operations.Operands.Instructions.AllocInstructions;
using MiniC.Operations.Operands.Instructions.ArithmeticInstructions;
using MiniC.Operations.Operands.Instructions.CompareInstructions;
using MiniC.Operations.Operands.Instructions.ConvertInstructions;
using MiniC.Scopes;

namespace MiniC.Generators
{
    public class AsmCodeWriter
    {
        private ArrayList _dataSection = new ArrayList();
        private ListingScopeTree _textSection = new ListingScopeTree("Text section", ListingScopeType.All);
        private IOperation _lastAddedOperation = null;
        
        public Stack<string> LoopStack = new Stack<string>();
        public Stack<string> IfStack = new Stack<string>();
        private Stack<string> funcStack = new Stack<string>();
        private Stack<ListingScopeTree> listingScopeStack = new Stack<ListingScopeTree>();
        private ListingScopeTree currentListingScope;

        private void addOperationToDataSection(IOperation op)
        {
            _dataSection.Add(op);
        }
        private void addOperationToTextSection(IOperation op)
        {
            currentListingScope.Add(op);
            _lastAddedOperation = op;
        }

        public ParseTreeProperty<SymbolType> Conversions;
        public SymbolType LastReferencedStructType = null;
        public ISymbol LastReferencedSymbol = null;

        public AsmCodeWriter(ParseTreeProperty<Scope> skopes, GlobalScope globalScope, 
            ParseTreeProperty<SymbolType> conversions, bool useRandomRegisterGetter = false)
        {
            scopes = skopes;
            GlobalScope = globalScope;
            Conversions = conversions;

            currentListingScope = _textSection;
            
            addOperationToDataSection(new DataSectionOperation());
            addOperationToTextSection(new TextSectionOperation());

            if (useRandomRegisterGetter)
            {
                AvaliableRegisters = new RandomRegisterGetter("r", 29);
                AvaliablePredicateRegisters = new RandomRegisterGetter("p", 4);
            }
            else
            {
                AvaliableRegisters = new ConsistentRegisterGetter("r", 29);
                AvaliablePredicateRegisters = new ConsistentRegisterGetter("p", 4);
            }
        }

        #region Registers work
        
        public Register LastAssignedRegister;
        public Register LastReferencedAddressRegister;
        public IRegisterGetter AvaliableRegisters;
        public IRegisterGetter AvaliablePredicateRegisters;

        public Register GetFreeRegister(int startIdx = 0)
        {
            return AvaliableRegisters.GetFreeRegister(startIdx);
        }

        public void FreeRegister(Register register)
        {
            AvaliableRegisters.FreeRegister(register);
        }

        public void FreeLastReferencedAddressRegister()
        {
            if (LastReferencedAddressRegister != null)
                FreeRegister(LastReferencedAddressRegister);
            LastReferencedAddressRegister = null;
        }

        public Register GetFreePredicateRegister()
        {
            return AvaliablePredicateRegisters.GetFreeRegister();
        }

        public void FreePredicateRegister(Register pRegister)
        {
            AvaliablePredicateRegisters.FreeRegister(pRegister);
        }
        
        #endregion

        #region Functions and stack offset stack
        
        private Stack<int> variablesOffsetStack = new Stack<int>();
        public int FuncParametersOffsetFromStackHead = 0;
        public GlobalScope GlobalScope { get; }

        public void PushFunc(string funcName)
        {
            funcStack.Push(funcName);
            listingScopeStack.Push(currentListingScope);
            variablesOffsetStack.Push(0);
            currentListingScope = new ListingScopeTree(funcName, ListingScopeType.Func);
        }

        public (string, int) PopFunc()
        {
            var lastListingScope = listingScopeStack.Pop();
            lastListingScope.Add(currentListingScope);
            currentListingScope = lastListingScope;
            
            var poppedFunc = funcStack.Pop();
            var poppedOffset = variablesOffsetStack.Pop();
            return (poppedFunc, poppedOffset);
        }

        public string GetCurrentFunc()
        {
            return funcStack.Peek();
        }

        public int GetCurrentStackOffset()
        {
            return variablesOffsetStack.Peek();
        }

        public int PushRegisterValueToStack(Register register, int size = -1)
        {
            var type = register.Type;
            var currentStackOffset = variablesOffsetStack.Pop();
            AddRegisterToMemWriting(Register.SP(), register, currentStackOffset.ToString());
            currentStackOffset += size == -1 ? type.Size : size;
            variablesOffsetStack.Push(currentStackOffset);
            return currentStackOffset;
        }

        #endregion
        
        #region Scopes work

        public ParseTreeProperty<Scope> scopes { get; }
        private Stack<Scope> scopesStack = new Stack<Scope>();

        public Scope PushScope(ParserRuleContext ctx)
        {
            scopesStack.Push(scopes.Get(ctx));
            return scopesStack.Peek();
        }

        public Scope PopScope()
        {
            return scopesStack.Pop();
        }

        public Scope GetCurrentScope()
        {
            return scopesStack.Peek();
        }

        public void PushLoop(string loopName)
        {
            listingScopeStack.Push(currentListingScope);
            currentListingScope = new ListingScopeTree(loopName, ListingScopeType.Loop);
            
            LoopStack.Push(loopName);
        }

        public string PopLoop()
        {
            var tmpScope = listingScopeStack.Pop();
            tmpScope.Add(currentListingScope);
            currentListingScope = tmpScope;

            return LoopStack.Pop();
        }

        public void PushIf(string ifName)
        {
            listingScopeStack.Push(currentListingScope);
            currentListingScope = new ListingScopeTree(ifName, ListingScopeType.If);
            
            IfStack.Push(ifName);
        }

        public string PopIf()
        {
            var tmpScope = listingScopeStack.Pop();
            tmpScope.Add(currentListingScope);
            currentListingScope = tmpScope;

            return IfStack.Pop();
        }
        
        #endregion

        #region Getting code

        public string GetCode()
        {
            var stringBuilder = new StringBuilder();
            foreach (IOperation variable in _dataSection)
                stringBuilder.AppendLine(variable.AsmString);
            stringBuilder.Append('\n');
            stringBuilder.Append(_textSection.AsmString);
            return stringBuilder.ToString();
        }
        
        public void WriteToFile(string filename = "../../../generated.S")
        {
            using var writer = new StreamWriter(File.Open(filename, FileMode.Create));
            writer.AutoFlush = true;
            // writer.Write(AllCode);
            writer.Write(GetCode());
        }

        #endregion

        #region Adding global variables

        public void AddGlobalVariable(VarSymbol symbol)
        {
            symbol.IsGlobal = true;
            symbol.BaseAddress = symbol.Name;
            addGlobalVariableRecursive(symbol.Name, symbol);
        }
        
        private void addGlobalVariableRecursive(string name, ISymbol symbol)
        {
            var defaultTypes = new string[] {"char", "int", "float"};
            if (defaultTypes.Contains(symbol.Type.Name))
            {
                if (symbol.Type.IsArray)
                    addGlobalEmptyArray(name, symbol.Type, symbol.ArraySize);
                else
                    addGlobalVariable(name, symbol.Type);
            }
            else
            {
                // _code += $"\n{name}:";
                AddLabel(name, false);
                var structSymbol = GlobalScope.FindStruct(symbol.Type);
                if (symbol.Type.IsArray)
                {
                    for (int i = 0; i < symbol.ArraySize; i++)
                    {
                        // _code += $"\n{name}_{i}:";
                        AddLabel($"{name}_{i}");
                        foreach (var symKeyValue in structSymbol.Table)
                        {
                            var symName = symKeyValue.Key;
                            var sym = symKeyValue.Value;
                            addGlobalVariableRecursive($"{name}_{symbol.Type.Name}_{symName}_{i}", sym);
                        }   
                    }
                }
                else
                {
                    foreach (var symKeyValue in structSymbol.Table)
                    {
                        var symName = symKeyValue.Key;
                        var sym = symKeyValue.Value;
                        addGlobalVariableRecursive($"{name}_{symbol.Type.Name}_{symName}", sym);
                    }   
                }
            }
        }
        
        private void addGlobalVariable(string name, SymbolType type)
        {
            // _variables += $"\n{name}:\n\t.{type.Name}\t0";
            addOperationToDataSection(new AddGlobalVariableOperation(name, type));
        }
        
        private void addGlobalEmptyArray(string name, SymbolType type, int capacity)
        {
            addOperationToDataSection(new AddGlobalArrayOperation(name, type, capacity));
        }

        #endregion

        #region Adding local variables

        public int AddEmptyLocalVariable(VarSymbol symbol)
        {
            var currentOffset = variablesOffsetStack.Pop();

            symbol.IsGlobal = false;
            symbol.BaseAddress = currentOffset.ToString();
            currentOffset = addLocalVariable(symbol, currentOffset);

            variablesOffsetStack.Push(currentOffset);
            return currentOffset;
        }

        private int addLocalVariable(ISymbol symbol, int currentOffset)
        {
            var defaultTypes = new string[] {"char", "int", "float"};
            // Если стандартный тип
            if (defaultTypes.Contains(symbol.Type.Name))
            {
                var memFunc = symbol.Type.MemFunc;
                // Если массив
                if (symbol.Type.IsArray)
                {
                    // Добавляем указатель на нулевой элемент (костыли ура вот указатели не делали теперь хлебаем)
                    var pointerRegister = GetFreeRegister();
                    AddAddingValueToRegister(pointerRegister, Register.SP(), currentOffset + 4);
                    pointerRegister.AddOffset((currentOffset + 4).ToString());
                    // _code += $"\n\tmemw(SP + #{currentOffset}) = {pointerRegister};";
                    AddRegisterToMemWriting(Register.SP(), pointerRegister, 
                        currentOffset.ToString());
                    FreeRegister(pointerRegister);
                    currentOffset += 4;
                    for (int i = 0; i < symbol.ArraySize; i++)
                    {
                        // _code += $"\n\t{memFunc}(SP + #{currentOffset}) = #0;";
                        AddValueToMemWriting(Register.SP(), 0, currentOffset.ToString());
                        currentOffset += symbol.Type.Size;
                    }   
                }
                // Если одно значение
                else
                {
                    // _code += $"\n\t{memFunc}(SP + #{currentOffset}) = #0;";
                    AddValueToMemWriting(Register.SP(), 0, currentOffset.ToString());
                    currentOffset += symbol.Type.Size;
                }
            }
            // Если структура
            else
            {
                var structSymbol = GlobalScope.FindStruct(symbol.Type);
                // Если массив
                if (symbol.Type.IsArray)
                {
                    // Добавляем указатель на нулевой элемент (костыли ура вот указатели не делали теперь хлебаем)
                    var pointerRegister = GetFreeRegister();
                    AddAddingValueToRegister(pointerRegister, Register.SP(), currentOffset + 4);
                    pointerRegister.AddOffset((currentOffset + 4).ToString());
                    // _code += $"\n\tmemw(SP + #{currentOffset}) = {pointerRegister};";
                    AddRegisterToMemWriting(Register.SP(), pointerRegister, 
                        currentOffset.ToString());
                    FreeRegister(pointerRegister);
                    currentOffset += 4;
                    for (int i = 0; i < symbol.ArraySize; i++)
                    {
                        foreach (var symKeyValue in structSymbol.Table)
                        {
                            var sym = symKeyValue.Value;
                            currentOffset = addLocalVariable(sym, currentOffset);
                        }
                    }
                }
                // Если одна структура
                else
                {
                    foreach (var symKeyValue in structSymbol.Table)
                    {
                        var sym = symKeyValue.Value;
                        currentOffset = addLocalVariable(sym, currentOffset);
                    }
                }
            }

            return currentOffset;
        }


        #endregion
        
        #region Adding function labels
        public void AddFunctionStart(string name)
        {
            var label = $"func_{name}_start";
            AddLabel(label);
            PushFunc(name);
        }

        public void AddFunctionEnd(string name)
        {
            var label = $"func_{name}_end";
            AddLabel(label);
            var op = new DeallocOperation(new DeallocReturnInstruction());
            addOperationToTextSection(op);
            PopFunc();
        }

        public void AddReturn(string funcName)
        {
            var label = $"func_{funcName}_end";
            AddJump(label);
        }

        public void AddReturnValue(Register sourceRegister)
        {
            AddRegisterToRegisterAssign(AvaliableRegisters.Zero, sourceRegister);
        }
        
        #endregion

        #region Allocating stack frame

        public void AddAllocateStackFrame4000()
        {
            var op = new AllocOperation(4000);
            addOperationToTextSection(op);
        }

        #endregion

        #region Adding basic labels

        public void AddLabel(string label, bool toTextSection = true)
        {
            var op = new LabelOperation(label);
            if (toTextSection)
                addOperationToTextSection(op);
            else
                addOperationToDataSection(op);
        }

        #endregion
        
        #region Adding loop labels
        public void AddLoopStart(string name)
        {
            var label = $"loop_{name}_start";
            AddLabel(label);
            PushLoop($"loop_{name}");
        }

        public void AddLoopEnd(string name)
        {
            var label = $"loop_{name}_end";
            AddLabel(label);
            PopLoop();
        }

        public void AddContinue(string loopName)
        {
            var label = $"loop_{loopName}_start";
            AddJump(label);
        }

        public void AddConditionalContinue(string loopName, Register pRegister, bool negate = false)
        {
            var label = $"loop_{loopName}_start";
            AddConditionalJump(pRegister, label, negate);
        }

        public void AddBreak(string loopName)
        {
            var label = $"loop_{loopName}_end";
            AddJump(label);
        }

        public void AddConditionalBreak(string loopName, Register pRegister, bool negate = false)
        {
            var label = $"loop_{loopName}_end";
            AddConditionalJump(pRegister, label, negate);
        }
        
        #endregion

        #region Adding if labels
        public void AddIfStart(string name)
        {
            var label = $"if_{name}_start";
            AddLabel(label);
            PushIf($"if_{name}");
        }

        public void AddIfEnd(string name)
        {
            var label = $"if_{name}_end";
            AddLabel(label);
            PopIf();
        }

        public void AddIfElse(string ifName)
        {
            var label = $"if_{ifName}_else";
            AddLabel(label);
        }

        public void AddJumpToElse(string ifName)
        {
            var label = $"if_{ifName}_else";
            AddJump(label);
        }
        
        public void AddConditionalJumpToElse(string ifName, Register pRegister, bool negate = false)
        {
            var label = $"if_{ifName}_else";
            AddConditionalJump(pRegister, label, negate);
        }

        public void AddJumpToIfEnd(string ifName)
        {
            var label = $"if_{ifName}_end";
            AddJump(label);
        }
        
        public void AddConditionalJumpToIfEnd(string ifName, Register pRegister, bool negate = false)
        {
            var label = $"if_{ifName}_end";
            AddConditionalJump(pRegister, label, negate);
        }
        
        #endregion

        #region Read-write variables to register

        public void AddRegisterToVariableWriting(VarSymbol variable, Register register)
        {
            var memRegister = GetFreeRegister();
            AddVariableAddressToRegisterReading(variable, memRegister);
            // Пока в этой функции 1 юсадж (в вариабл дефинишн, то читить этот регистр не надо, иначе оффсеты обнуляются у memRegiser
            // FreeLastReferencedAddressRegister();
            var memInstr = new MemInstruction(memRegister);
            var op = new MemWriteOperation(memInstr, register);
            addOperationToTextSection(op);
            // // LastReferencedVariable = variable;
            FreeRegister(memRegister);
        }
        
        public void AddVariableAddressToRegisterReading(VarSymbol variable, Register register)
        {
            register.Type = variable.Type;
            IOperation op;
            register.AddOffset(variable.BaseAddress);
            if (variable.IsGlobal)
            {
                // _code += $"\n\t{register} = ##{variable.BaseAddress};";
                op = new GlobalVariableAddressToRegisterOperation(register, variable);
            }
            else
            {
                // _code += $"\n\t{register} = add(SP, #{variable.BaseAddress})";
                var baseAddr = new IntConstOperand(variable.BaseAddress);
                var opRhs = new AddInstruction(Register.SP(), baseAddr);
                op = new ArithmeticOperation(register, opRhs); 
            }
            addOperationToTextSection(op);
            LastReferencedAddressRegister = register;
        }
        
        public void AddRegisterToVariableWritingWithOffset(VarSymbol variable, Register register, string offset)
        {
            var memRegister = GetFreeRegister();
            memRegister.AddOffset(variable.BaseAddress);
            // memRegister.AddOffset(offset);
            if (variable.IsGlobal)
            {
                // _code += $"\n\t{memRegister} = ##{variable.BaseAddress};";
                var addrReadOp = new GlobalVariableAddressToRegisterOperation(memRegister, variable);
                addOperationToTextSection(addrReadOp);
            }
            else
            {
                // _code += $"\n\t{memRegister} = add(SP, #{variable.BaseAddress});";
                var addrOperand = new IntConstOperand(variable.BaseAddress);
                var addInstr = new AddInstruction(Register.SP(), addrOperand);
                var addrReadOp = new ArithmeticOperation(memRegister, addInstr);
                addOperationToTextSection(addrReadOp);
            }
            // _code += $"\n\t{memFunc}({memRegister} + #{offset}) = {register};";
            var memInstr = new MemInstruction(memRegister, offset);
            var op = new MemWriteOperation(memInstr, register);
            addOperationToTextSection(op);
            FreeRegister(memRegister);
        }

        public void AddMemToRegisterReading(Register addressRegister, SymbolType type, Register destRegister, string offsetValue = "")
        {
            destRegister.Type = type;
            // destRegister.AddRangeOfOffsets(addressRegister.Offsets);
            // destRegister.AddOffset(offsetValue);
            var memInstr = new MemInstruction(addressRegister, offsetValue);
            var op = new MemReadOperation(destRegister, memInstr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddRegisterToMemWriting(Register addressRegister, Register sourceRegister, string offsetValue = "")
        {
            var memInsr = new MemInstruction(addressRegister, offsetValue);
            var op = new MemWriteOperation(memInsr, sourceRegister);
            addOperationToTextSection(op);
        }

        public void AddValueToMemWriting(Register addressRegister, int value, string offsetValue = "")
        {
            var memInsr = new MemInstruction(addressRegister, offsetValue);
            var op = new MemWriteOperation(memInsr, value);
            addOperationToTextSection(op);
        }

        #endregion
        
        #region Working with registers
        public void AddValueToRegisterAssign(Register register, string value, SymbolType type)
        {
            register.Type = type;
            ConstOperand rhs;
            if (type.Name == "float")
            {
                AddInlineComment($"{value} as hex (sfmake works poorly)");
                rhs = new FloatConstOperand(float.Parse(value));
            }
            else if (type.Name == "int")
                rhs = new IntConstOperand(value);
            else  // char
                rhs = new CharConstOperand(value);
            var op = new ValueToRegisterOperation(register, rhs);
            addOperationToTextSection(op);
            LastAssignedRegister = register;
        }

        public void AddRegisterToRegisterAssign(Register lhs, Register rhs)
        {
            lhs.Type = rhs.Type;
            var op = new RegisterToRegisterOperation(lhs, rhs);
            addOperationToTextSection(op);
            LastAssignedRegister = lhs;
        }
        
        public void AddConditionalRegisterToRegisterAssign(Register pRegister, Register destRegister, 
            Register sourceRegisterIfTrue, Register sourceRegisterIfFalse)
        {
            destRegister.Type = sourceRegisterIfTrue.Type;    // у true и false одинаковые типы должны быть
            var ifTrueOp = new RegisterToRegisterOperation(destRegister, sourceRegisterIfTrue);
            var ifFalseOp = new RegisterToRegisterOperation(destRegister, sourceRegisterIfFalse);
            var condOpIfTrue = new ConditionalOperation(pRegister, ifTrueOp, false);
            var condOpIfFalse = new ConditionalOperation(pRegister, ifFalseOp, true);
            addOperationToTextSection(condOpIfTrue);
            addOperationToTextSection(condOpIfFalse);
            LastAssignedRegister = destRegister;
        }
        
        #endregion
        
        #region ALU function
        public void AddAddingRegisterToRegister(Register lhs, Register s1, Register s2)
        {
            var resultType = SymbolType.GetBigger(s1.Type, s2.Type);
            lhs.Type = resultType;
            var addInstruction = new AddInstruction(s1, s2);
            var operation = new ArithmeticOperation(lhs, addInstruction);
            addOperationToTextSection(operation);
            LastAssignedRegister = lhs;
        }

        public void AddAddingValueToRegister(Register lhs, Register r1, int value)
        {
            lhs.Type = SymbolType.GetType("int");
            var val = new IntConstOperand(value);
            var addInstruction = new AddInstruction(r1, val);
            var operation = new ArithmeticOperation(lhs, addInstruction);
            addOperationToTextSection(operation);
            LastAssignedRegister = lhs;
        }
        
        public void AddSubRegisterFromRegister(Register lhs, Register s1, Register s2)
        {
            
            var resultType = SymbolType.GetBigger(s1.Type, s2.Type);
            lhs.Type = resultType;
            var addInstruction = new SubInstruction(s1, s2);
            var operation = new ArithmeticOperation(lhs, addInstruction);
            addOperationToTextSection(operation);
            LastAssignedRegister = lhs;
        }

        public void AddNegateRegister(Register destRegister, Register sourceRegister)
        {
            destRegister.Type = sourceRegister.Type;
            var negInstruction = new NegInstruction(sourceRegister);
            var operation = new ArithmeticOperation(destRegister, negInstruction);
            addOperationToTextSection(operation);
            LastAssignedRegister = destRegister;
        }
        
        public void AddRegisterMpyRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var mpyInstr = new MultiplyInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, mpyInstr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }
        
        public void AddRegisterDivRegister(Register destRegister, Register r1, Register r2)
        {
            var type = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = type;
            if (type.Name == "float")
                divWithRegisterSaving(destRegister, r1, r2, 7, "__hexagon_divsf3");
            else 
                divWithRegisterSaving(destRegister, r1, r2, 5, "__hexagon_divsi3");
        }
        
        public void AddRegisterModRegister(Register destRegister, Register r1, Register r2)
        {
            divWithRegisterSaving(destRegister, r1, r2, 2, "__hexagon_umodsi3");
        } 
        
        private void divWithRegisterSaving(Register destRegister, Register r1, Register r2, int maxSavedRegisterIdx, 
            string divFunc)
        {
            // Регистры с 0 по maxSavedRegisterIdx участвуют в макросе деления, так что сохраняем их
            AddComment($"Saving r0-r{maxSavedRegisterIdx} before sfdiv");
            var savedRegisters = new List<Register>();
            for (int i = 0; i < maxSavedRegisterIdx+1; i++)
            {
                var r = AvaliableRegisters[i];
                var sr = GetFreeRegister(maxSavedRegisterIdx+1);
                AddRegisterToRegisterAssign(sr, r);
                savedRegisters.Add(sr);
            }

            // Кладем в регистры r0 и r1 значения для деления
            AddComment($"Calling {divFunc}");
            var reg0 = AvaliableRegisters[0];
            var reg1 = AvaliableRegisters[1];
            AddRegisterToRegisterAssign(reg0, r1);
            AddRegisterToRegisterAssign(reg1, r2);
            
            // Вызываем макрос
            // _code += $"\n\tcall {divFunc};";
            AddCall(divFunc, false);

            // Результат деления сейчас в r0, поэтому восстанавливаем все регистры кроме нулевого
            AddComment($"Restoring r1-r{maxSavedRegisterIdx} registers");
            for (int i = 1; i < maxSavedRegisterIdx+1; i++)
            {
                var r = AvaliableRegisters[i];
                AddRegisterToRegisterAssign(r, savedRegisters[i]);
                FreeRegister(savedRegisters[i]);
            }
            
            // Копируем значение из r0 в первый свободный регистр и восстанавливаем r0
            if (destRegister.Name == "r0")
                AddComment($"Dest register set to r0, no need to restore its prev value, {divFunc} is over");
            else
            {
                AddComment("Copying result and restoring r0");
                AddRegisterToRegisterAssign(destRegister, reg0);
                AddRegisterToRegisterAssign(reg0, savedRegisters[0]);
            }
            FreeRegister(savedRegisters[0]);
            LastAssignedRegister = destRegister;
        }
        
        #endregion

        #region Converting types

        public void AddIntRegisterToFloatConvert(Register destRegister, Register sourceRegister)
        {
            if (sourceRegister.Type.Name == "float")
                return;
            destRegister.Type = SymbolType.GetType("float");
            var convInstr = new IntToFloatConvertInstruction(sourceRegister);
            var operation = new ConvertOperation(destRegister, convInstr);
            addOperationToTextSection(operation);
            LastAssignedRegister = destRegister;
        }

        public void AddFloatRegisterToIntConvert(Register destRegister, Register sourceRegister)
        {
            if (sourceRegister.Type.Name == "int" || sourceRegister.Type.Name == "char")
                return;
            destRegister.Type = SymbolType.GetType("int");
            var convInstr = new FloatToIntConvertInstruction(sourceRegister);
            var operation = new ConvertOperation(destRegister, convInstr);
            addOperationToTextSection(operation);
            LastAssignedRegister = destRegister;
        }

        public void ConvertRegisterToType(Register destRegister, Register sourceRegister, SymbolType type)
        {
            if (type.Name == "float")
                AddIntRegisterToFloatConvert(destRegister, sourceRegister);
            else if (type.Name == "char" || type.Name == "int")
                AddFloatRegisterToIntConvert(destRegister, sourceRegister);
            else 
                throw new CodeGenerationException($"Unknown type for conversion \"{type.Name}\"");
        }

        #endregion
        
        #region Bit manipulation functions
        public void AddRegisterAndRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var instr = new AndInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddRegisterOrRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var instr = new OrInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddRegisterXorRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var instr = new XorInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddRegisterRightShiftRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var instr = new RShiftInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddRegisterLefShiftRegister(Register destRegister, Register r1, Register r2)
        {
            var resultType = SymbolType.GetBigger(r1.Type, r2.Type);
            destRegister.Type = resultType;
            var instr = new LShiftInstruction(r1, r2);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }

        public void AddNotRegister(Register destRegister, Register sourceRegister)
        {
            destRegister.Type = sourceRegister.Type;
            var instr = new NotInstruction(sourceRegister);
            var op = new ArithmeticOperation(destRegister, instr);
            addOperationToTextSection(op);
            LastAssignedRegister = destRegister;
        }
        
        #endregion
        
        #region Adding compares
        public void AddCompareRegisterEqRegister(Register pRegister, Register r1, Register r2, bool negate = false)
        {
            var type = SymbolType.GetBigger(r1.Type, r2.Type);
            var eqInstruction = new EqInstruction(r1, r2, negate);
            var op = new CompareOperation(pRegister, eqInstruction);
            addOperationToTextSection(op);
            if (type.Name == "float" && negate) 
            {
                AddComment("Can't do !sfcmp.eq, so inverting resulting pregister");
                AddNotRegister(pRegister, pRegister);
            }
        }
        
        public void AddCompareRegisterEqNumber(Register pRegister, Register register, string value, bool negate = false)
        {
            var type = register.Type;
            if (type.Name == "float")
            {
                var flRegister = GetFreeRegister();
                AddValueToRegisterAssign(flRegister, value, type);
                AddCompareRegisterEqRegister(pRegister, register, flRegister, negate);
                FreeRegister(flRegister);
            }
            else
            {
                var valueOperand = new IntConstOperand(value);
                var eqInstr = new EqInstruction(register, valueOperand, negate);
                var op = new CompareOperation(pRegister, eqInstr);
                addOperationToTextSection(op);
            }
        }
        
        public void AddCompareRegisterGeRegister(Register pRegister, Register r1, Register r2)
        {
            var compareInstruction = new GeInstruction(r1, r2);
            var op = new CompareOperation(pRegister, compareInstruction);
            addOperationToTextSection(op);
        }
        
        public void AddCompareRegisterGtRegister(Register pRegister, Register r1, Register r2)
        {
            var compareInstruction = new GtInstruction(r1, r2);
            var op = new CompareOperation(pRegister, compareInstruction);
            addOperationToTextSection(op);
        }
        
        public void AddCompareRegisterLeRegister(Register pRegister, Register r1, Register r2)
        {
            // LE делается через GE простым свапом параметров
            var compareInstruction = new LeInstruction(r1, r2);
            var op = new CompareOperation(pRegister, compareInstruction);
            addOperationToTextSection(op);
        }
        
        public void AddCompareRegisterLtRegister(Register pRegister, Register r1, Register r2)
        {
            // LT делается через GT простым свапом параметров
            var compareInstruction = new LtInstruction(r1, r2);
            var op = new CompareOperation(pRegister, compareInstruction);
            addOperationToTextSection(op);
        }
        
        #endregion
        
        #region Adding calls, jumps and returns
        public void AddJump(string label)
        {
            var op = new JumpOperation(label);
            addOperationToTextSection(op);
        }
        
        public void AddConditionalJump(Register pRegister, string label, bool negate = false)
        {
            var jumpOperation = new JumpOperation(label);
            var op = new ConditionalOperation(pRegister, jumpOperation, negate);
            addOperationToTextSection(op);
        }

        public void AddCall(string funcName, bool addFuncPrefixSuffix = true)
        {
            string labelName = addFuncPrefixSuffix ? $"func_{funcName}_start" : funcName;
            var op = new JumpOperation(labelName, true);
            addOperationToTextSection(op);
        }
        
        #endregion

        #region Adding comments
        public void AddInlineComment(string comment)
        {
            _lastAddedOperation.InlineComment = comment;
        }

        public void AddComment(string comment, bool withTab = true)
        {
            _lastAddedOperation.AddLowerComment(comment, withTab);
        }
        
        #endregion

    }
}