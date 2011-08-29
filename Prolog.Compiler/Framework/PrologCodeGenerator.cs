//------------------------------------------------------------------------------
// <copyright file="PrologCodeGenerator.cs" company="Axiom">
//     
//      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
//     
//      The use and distribution terms for this source code are contained in the file
//      named license.txt, which can be found in the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by the
//      terms of this license.
//     
//      You must not remove this notice, or any other, from this software.
//     
// </copyright>                                                                
//------------------------------------------------------------------------------



using System;
using Axiom.Compiler.CodeObjectModel;
using System.Collections;
using Axiom.Compiler.Framework.Generators;
using Axiom.Runtime;



namespace Axiom.Compiler.Framework
{	
	public class PrologCodeGenerator :	IPrologCodeGenerator
	{
        private PrologVariableDictionary _dictionary = null;
        private ArrayList _methods = new ArrayList();
        private int headArity = 0;
        private AMGenerator _generator = null;
        private int _currArgN = 0;

		public PrologCodeGenerator ()
		{
		
		}
		
		public void GenerateCodeFromUnit (PrologCodeUnit unit, ArrayList instructions)
		{
            _generator = new AMGenerator(instructions);

            foreach (PrologCodeTerm term in unit.Terms)
            {
                ArrayList inst = new ArrayList();
                   
                if (term is PrologCodeClause)
                {
                    GenerateCodeFromClause((PrologCodeClause)term, inst);
                    instructions.AddRange(inst);
                }
                else if (term is PrologCodePredicate)
                {
                    PrologCodeClause c = new PrologCodeClause((PrologCodePredicate)term);
                    GenerateCodeFromClause(c, inst);
                    instructions.AddRange(inst);

                }
                else
                {
                    throw new PrologCompilerException("Unknown term type: " + term.ToString());
                }
            }
		}
		
		public void GenerateCodeFromClause (PrologCodeClause clause, ArrayList instructions)
		{
            /* Do we need to allocate an environment? */
            bool hasEnvironment = clause.Goals.Count > 1;

            /* Initialize variable dictionary */
            _dictionary = new PrologVariableDictionary();

            /* Build the variable dictionary for this clause */
            _dictionary.Build(clause);

            /* Free all registers */
            PrologRegisterTable registers = PrologRegisterTable.Instance;
            registers.FreeAllRegisters();

            /* Prepare head variables for code generation */
            int reg = 0;
            if (clause.Head.Arity > 0)
            {
                headArity = clause.Head.Arity;
                foreach (PrologCodeTerm argument in clause.Head.Arguments)
                {
                    if (argument is PrologCodeVariable)
                    {
                        PrologCodeVariable var = (PrologCodeVariable)argument;
                        PrologVariableDictionaryEntry entry = _dictionary.GetVariable(var.Name);
                        if (entry != null)
                        {
                            if (entry.IsTemporary && entry.TemporaryIndex == -1)
                            {
                                entry.IsReferenced = true;
                                _dictionary.AllocateTemporaryVariable(entry, reg);
                            }
                        }
                        //BUG: reg++;
                    }
                    reg++;
                }
            }

            /* Prepare first goal variables */
            int xreg = 0;
            PrologCodeTerm fg = null;
            if (clause.Goals.Count > 0)
            {
                fg = (PrologCodeTerm)clause.Goals[0];
            }
            if (fg is PrologCodePredicate)
            {
                PrologCodePredicate firstGoal = (PrologCodePredicate)fg;
                if (firstGoal.Name == "!")
                {
                    hasEnvironment = true;
                }
                if (firstGoal.Arity > 0)
                {
                    foreach (PrologCodeTerm variable in firstGoal.Arguments)
                    {
                        if (variable is PrologCodeVariable)
                        {
                            PrologVariableDictionaryEntry entry = _dictionary.GetVariable(((PrologCodeVariable)variable).Name);
                            if (entry != null)
                            {
                                if (entry.IsTemporary && entry.TemporaryIndex == -1)
                                {
                                    if (!registers.InUse(xreg))
                                    {
                                        _dictionary.AllocateTemporaryVariable(entry, xreg);
                                    }
                                }
                            }
                        }
                        xreg++;
                    }
                }
            }
                /* Reserve required registers */
            for (int i = 0; i < Math.Max(reg, xreg); i++)
            {
                 registers.AllocateRegister(i);
            }

            /* Emit predicate label */
            _generator.DeclareProcedure(clause.Head.Name, clause.Head.Arity);

            /* Allocate an environment if needed */
            if (hasEnvironment)
            {
                _generator.Emit(OpCodes.Allocate);
            }

            /* Compile clause head */
            CompileClauseHead(clause.Head, instructions);

            /* Set current goal to 1 */
            _dictionary.CurrentGoalIndex = 1;

            if (clause.Goals.Count == 0)
            {
                _generator.EndProcedure();

                /* Reset variable dictionary */
                _dictionary.Reset();
                instructions = _generator.Instructions;
                return;
            }

            /* Compile first goal */
            CompileGoal(fg, instructions);
            _dictionary.CurrentGoalIndex++;

            /* Compile the rest of the goals */
            for (int goalIndex = 1; goalIndex < clause.Goals.Count; goalIndex++)
            {
                PrologCodeTerm goal = (PrologCodeTerm)clause.Goals[goalIndex];
                InitializeGoalTemporaryVariables(goal);

                /* reserve registers */
                for (int i = 0; i < reg; i++)
                {
                    registers.AllocateRegister(i);
                }

                /* Clear temporary index of permanent variables */
                _dictionary.ClearTempIndexOfPermanentVariables();

                /* Compile goal */
                CompileGoal(goal, instructions);

                /* Advance to next goal */
                _dictionary.CurrentGoalIndex += 1;
            }
            /* Reset instruction set, code pointer, and variable
             * dictionary.
             */
            _dictionary.Reset();
		
		}

        public void GenerateCodeFromPredicate(PrologCodePredicate p, ArrayList a)
		{
            PrologCodeClause clause = new PrologCodeClause(p);
            GenerateCodeFromClause(clause, a);

        }

        #region Head compilation helper methods
        private void CompileClauseHead(PrologCodeTerm head, ArrayList instructions)
        {
            if (head is PrologCodePredicate)
            {
                PrologCodePredicate headPredicate = (PrologCodePredicate)head;

                if (headPredicate.Arity == 0)
                {
                    /* Do nothing */
                }
                else
                {
                    CompileHeadArguments(((PrologCodePredicate)head).Arguments);
                }
            }
            else if (head is PrologCodeNonEmptyList)
            {
                ArrayList headListArguments = new ArrayList();
                PrologCodeNonEmptyList NEList = (PrologCodeNonEmptyList)head;
                headListArguments.Add(NEList.Head);
                headListArguments.Add(NEList.Tail);
                CompileHeadArguments(headListArguments);
            }
            else if (head is PrologCodeVariable)
            {
                throw new PrologCompilerException("Clause head cannot be a variable.");
            }
            else if (head is PrologCodeIntegerAtom || head is PrologCodeFloatAtom)
            {
                throw new PrologCompilerException("Clause head cannot be a number.");
            }

        }

        private void CompileHeadArguments(ArrayList arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                _currArgN = i;
                PrologCodeTerm arg = (PrologCodeTerm)arguments[i];

                if (arg is PrologCodeNilAtom || arg is PrologCodeEmptyList)
                {
                    _generator.Emit(OpCodes.Get_Constant, "[]", X(i));
                }
                else if (arg is PrologCodeAtom)
                {
                    if (arg is PrologCodeConstantAtom)
                    {
                        _generator.Emit(OpCodes.Get_Constant, ((PrologCodeConstantAtom)arg).Value, X(i));
                    }
                    else if (arg is PrologCodeIntegerAtom)
                    {
                         _generator.Emit(OpCodes.Get_Constant, ((PrologCodeIntegerAtom)arg).Value.ToString(), X(i));
                    }
                    else if (arg is PrologCodeFloatAtom)
                    {
                        _generator.Emit(OpCodes.Get_Constant, ((PrologCodeFloatAtom)arg).Value.ToString(), X(i));
                    }
                    else if (arg is PrologCodeStringAtom)
                    {
                        _generator.Emit(OpCodes.Get_Constant, ((PrologCodeStringAtom)arg).Value, X(i));
                    }
                }
                else if (arg is PrologCodeVariable)
                {
                    if (_dictionary.GoalCount == 0)
                    {
                        // warning: singleton variable
                    }
                    PrologVariableDictionaryEntry entry = _dictionary.GetVariable(((PrologCodeVariable)arg).Name);
                    if (entry.IsTemporary)
                    {
                        if (entry.IsReferenced && entry.TemporaryIndex != i)
                        {
                            _generator.Emit(OpCodes.Get_Value, X(entry.TemporaryIndex), X(i));
                        }
                    }
                    else
                    {
                        if (entry.IsReferenced)
                        {
                            _generator.Emit(OpCodes.Get_Value, Y(entry.PermanentIndex), X(i));
                        }
                        else
                        {
                            _generator.Emit(OpCodes.Get_Variable, Y(entry.PermanentIndex), X(i));
                        }
                    }
                }
                else if (arg is PrologCodeNonEmptyList)
                {
                    _generator.Emit(OpCodes.Get_List, X(i));
                    ArrayList listArguments = new ArrayList();
                    PrologCodeNonEmptyList NEList = (PrologCodeNonEmptyList)arg;
                    listArguments.Add(NEList.Head);
                    listArguments.Add(NEList.Tail);
                    CompileStructArguments(listArguments);
                }
                else if (arg is PrologCodePredicate)
                {
                    PrologCodePredicate structure = (PrologCodePredicate)arg;
                    _generator.Emit(OpCodes.Get_Structure, structure.Name + "/" + structure.Arity, X(i));
                    CompileStructArguments(structure.Arguments);
                }
                else
                {
                    throw new PrologCompilerException("Unknown argument type (" + arg.GetType().ToString() + ") in head arguments");
                }
            }
        }

        private void CompileStructArguments(ArrayList arguments)
        {
            ArrayList records = new ArrayList();
            // TODO: Why is it 20 here? was there a maximum number of records originally.
            for (int i = 0; i < 20; i++)
            {
                records.Add(new Record());
            }

            int nRecs = 0;
            for (int i = 0; i < arguments.Count; i++)
            {
                PrologCodeTerm term = (PrologCodeTerm)arguments[i];

                if (term is PrologCodeNilAtom)
                {
                    _generator.Emit(OpCodes.Unify_Constant, "[]");
                }
                else if (term is PrologCodeAtom)
                {
                    if (term is PrologCodeConstantAtom)
                    {
                        _generator.Emit(OpCodes.Unify_Constant, ((PrologCodeConstantAtom)term).Value);
                    }
                    else if (term is PrologCodeIntegerAtom)
                    {
                        _generator.Emit(OpCodes.Unify_Constant, ((PrologCodeIntegerAtom)term).Value.ToString());
                    }
                    else if (term is PrologCodeFloatAtom)
                    {
                        _generator.Emit(OpCodes.Unify_Constant, ((PrologCodeFloatAtom)term).Value.ToString());
                    }
                    else if (term is PrologCodeStringAtom)
                    {
                        _generator.Emit(OpCodes.Unify_Constant, ((PrologCodeStringAtom)term).Value);
                    }
                }
                else if (term is PrologCodeVariable)
                {
                    PrologVariableDictionaryEntry entry = _dictionary.GetVariable(((PrologCodeVariable)term).Name);
                    if (entry.IsReferenced)
                    {
                        if (entry.IsTemporary)
                        {
                            if (entry.IsGlobal)
                            {
                                _generator.Emit(OpCodes.Unify_Value, X(entry.TemporaryIndex));
                            }
                            else
                            {
                                // TODO: maybe this should be unify_variable
                                _generator.Emit(OpCodes.Unify_Local_Value, X(entry.TemporaryIndex));
                                entry.IsGlobal = true;
                            }
                        }
                        else
                        {
                            if (entry.IsGlobal)
                            {
                                _generator.Emit(OpCodes.Unify_Value, Y(entry.PermanentIndex));
                            }
                            else
                            {
                                _generator.Emit(OpCodes.Unify_Local_Value, Y(entry.PermanentIndex));
                            }
                        }
                    }
                    // not referenced
                    else
                    {
                        if (entry.IsTemporary)
                        {
                            if (entry.Occurrences == 1)
                            {
                                _generator.Emit(OpCodes.Unify_Void, "1");
                            }
                            else
                            {   // used to be i < entry.TemporaryIndex
                                if (_currArgN < entry.TemporaryIndex && entry.TemporaryIndex < headArity)
                                {
                                    entry.TemporaryIndex = PrologRegisterTable.Instance.FindRegister();
                                }
                                _generator.Emit(OpCodes.Unify_Variable, X(entry.TemporaryIndex));
                            }
                        }
                        else
                        {
                            _generator.Emit(OpCodes.Unify_Variable, Y(entry.PermanentIndex));
                        }
                        entry.IsGlobal = true;
                    }
                }
                else if (term is PrologCodeEmptyList)
                {
                    _generator.Emit(OpCodes.Unify_Constant, "[]");
                }
                else
                {
                    ((Record)records[nRecs]).Term = term;
                    ((Record)records[nRecs]).TemporaryIndex = PrologRegisterTable.Instance.FindRegister();
                    _generator.Emit(OpCodes.Unify_Variable, X(((Record)records[nRecs]).TemporaryIndex));
                    nRecs++;
                }
            }
            for (int i = 0; i < nRecs; i++)
            {
                Record r = (Record)records[i];
                if (r.Term is PrologCodeNonEmptyList)
                {
                    _generator.Emit(OpCodes.Get_List, X(r.TemporaryIndex));
                    PrologRegisterTable.Instance.FreeRegister(r.TemporaryIndex);
                    ArrayList listArguments = new ArrayList();
                    PrologCodeNonEmptyList NEList = (PrologCodeNonEmptyList)r.Term;
                    listArguments.Add(NEList.Head);
                    listArguments.Add(NEList.Tail);
                    CompileStructArguments(listArguments);
                }
                else if (r.Term is PrologCodePredicate)
                {
                    PrologCodePredicate structure = (PrologCodePredicate)r.Term;
                    _generator.Emit(OpCodes.Get_Structure, structure.Name + "/" + structure.Arity, X(r.TemporaryIndex));
                    CompileStructArguments(structure.Arguments);
                }
                
                else
                {
                    throw new PrologCompilerException("Unknown argument type (" + r.Term.GetType().ToString() + ") in structure arguments");
                }
            }
        }

        internal class Record
        {
            public PrologCodeTerm Term;
            public int TemporaryIndex;

            public Record()
            {
                Term = null;
                TemporaryIndex = -1;
            }
        }

        private string X(int i)
        {
            return "X" + i;
        }

        private string Y(int i)
        {
            return "Y" + i;
        }
        #endregion

        #region Goal compilation helper methods
        private void CompileGoal(PrologCodeTerm goal, ArrayList instructions)
        {
            if (goal is PrologCodePredicate && ((PrologCodePredicate)goal).Arity == 0)
            {
                PrologCodePredicate goalPredicate = (PrologCodePredicate)goal;
                if (goalPredicate.Arity == 0)
                {
                    if (goalPredicate.Name == "!")
                    {
                        _generator.Emit(OpCodes.Cut);
                        if (_dictionary.InLastGoal)
                        {
                            _generator.Emit(OpCodes.Deallocate);
                            _generator.EndProcedure();
                        }
                        return;
                    }
                    if (goalPredicate.Name == "fail")
                    {
                        _generator.Emit(OpCodes.Fail);
                        _generator.EndProcedure();
                        return;
                    }
                    // TODO: handle methods here...
                    CompileCall(goalPredicate);
                }
            }
            else if (goal is PrologCodeVariable)
            {
                CompileGoalVariable((PrologCodeVariable)goal, 0);
                if (_dictionary.InLastGoal)
                {
                   
                        if (_dictionary.CurrentGoalIndex > 1)
                        {
                            _generator.Emit(OpCodes.Deallocate);
                        }
                        _generator.EmitExecuteVar(((PrologCodeVariable)goal).Name, 0);
                }
                else
                {

                    _generator.EmitCallVar(((PrologCodeVariable)goal).Name, 0);
                    
                }
            }
            else if (goal is PrologCodeNonEmptyList)
            {
                // TODO: compile list arguments, then call ./2
            }
            else if (goal is PrologCodeIntegerAtom || goal is PrologCodeFloatAtom)
            {
                throw new PrologCompilerException("Clause goal cannot be a number.");
            }
            else if (goal is PrologCodePredicate)
            {
                CompileGoalArguments(((PrologCodePredicate)goal).Arguments);
                CompileCall(goal);
            }
        }

        private void CompileMethod(PrologCodeTerm method)
        {
            PrologCodePredicate predicate = (PrologCodePredicate)method;

            _generator.EmitFCall(predicate.MethodInfo.PredicateName,
                                 predicate.MethodInfo.MethodName,
                                 predicate.MethodInfo.AssemblyName,
                                 predicate.MethodInfo.Class);

            if (_dictionary.InLastGoal)
            {
                if (_dictionary.GoalCount > 2)
                {
                    _generator.Emit(OpCodes.Deallocate);
                }
                // Emit 'proceed'
                _generator.EndProcedure();
            }
        }

        private void CompileGoalVariable(PrologCodeVariable var, int i)
        {
            PrologVariableDictionaryEntry entry = _dictionary.GetVariable(var.Name);
            if (entry.IsTemporary)
            {
                if (entry.TemporaryIndex != i)
                {
                    ResolveConflicts(var, i);
                }
                if (entry.IsReferenced)
                {
                    if (entry.TemporaryIndex != i)
                    {
                        _generator.Emit(OpCodes.Put_Value, X(entry.TemporaryIndex), X(i));

                    }
                }
                else
                {
                    if (entry.TemporaryIndex != i)
                    {
                        _generator.Emit(OpCodes.Put_Variable, X(entry.TemporaryIndex), X(i));

                    }
                    else
                    {
                        _generator.Emit(OpCodes.Put_Variable, X(i), X(i));

                    }
                    entry.IsGlobal = true;
                }
            }
            else
            {
                ResolveConflicts(var, i);
                if (entry.IsReferenced)
                {
                    if (entry.IsUnsafe && !entry.IsGlobal &&
                        _dictionary.InLastGoal)
                    {
                        _generator.Emit(OpCodes.Put_Unsafe_Value, Y(entry.PermanentIndex), X(i));
                        entry.IsUnsafe = false;
                    }
                    else
                    {
                        if (entry.TemporaryIndex != -1)
                        {
                            _generator.Emit(OpCodes.Put_Value, X(entry.TemporaryIndex), X(i));
                        }
                        else
                        {
                            _generator.Emit(OpCodes.Put_Value, Y(entry.PermanentIndex), X(i));
                        }
                    }
                }
                else
                {
                    _generator.Emit(OpCodes.Put_Variable, Y(entry.PermanentIndex), X(i));
                }

                if (entry.TemporaryIndex == -1)
                {
                    entry.TemporaryIndex = i;
                }
                else
                {
                    // No
                }
            }
        }

        private void CompileGoalArguments(ArrayList arguments)
        {
            for (int i = 0; i < arguments.Count; i++)
            {
                PrologCodeTerm term = (PrologCodeTerm)arguments[i];

                // TODO: check it's a nil atom then handle put conflicts
                if (term is PrologCodeEmptyList)
                {
                    ResolveConflicts(term, i);
                    _generator.Emit(OpCodes.Put_Constant, "[]", X(i));
                }

                else if (term is PrologCodeAtom)
                {
                    // Handle put conflicts here...
                    ResolveConflicts(term, i);
                    if (term is PrologCodeConstantAtom)
                    {
                        _generator.Emit(OpCodes.Put_Constant, ((PrologCodeConstantAtom)term).Value, X(i));
                    }
                    else if (term is PrologCodeStringAtom)
                    {
                        _generator.Emit(OpCodes.Put_Constant, ((PrologCodeStringAtom)term).Value, X(i));
                   }
                    else if (term is PrologCodeIntegerAtom)
                    {
                        _generator.Emit(OpCodes.Put_Constant, ((PrologCodeIntegerAtom)term).Value.ToString(), X(i));
                    }
                    else if (term is PrologCodeFloatAtom)
                    {
                        _generator.Emit(OpCodes.Put_Constant, ((PrologCodeFloatAtom)term).Value.ToString(), X(i));
                    }
                }
                else if (term is PrologCodeVariable)
                {
                    CompileGoalVariable((PrologCodeVariable)term, i);
                }
                else if (term is PrologCodePredicate || term is PrologCodeNonEmptyList)
                {
                    ResolveConflicts(term, i);
                    CompileGoalRecord(term, i);
                }
            }
        }

        private bool ResolveConflicts(PrologCodeTerm term, int index)
        {
            PrologVariableDictionaryEntry entry = _dictionary.GetVariable(index);

            if (_dictionary.CurrentGoalIndex != 1 || entry == null || entry.LastGoalArgument < index)
            {
                PrologRegisterTable.Instance.AllocateRegister(index);
                return false;
            }
            if (term is PrologCodePredicate)
            {
                PrologCodePredicate predicate = (PrologCodePredicate)term;
                for (int i = index + 1; i < entry.LastGoalArgument; i++)
                {
                    if (predicate.Name == entry.Name &&
                        (_dictionary.GetVariable(i) == null || ResolveConflicts(term, i)))
                    {
                        entry.TemporaryIndex = i;
                        _generator.Emit(OpCodes.Put_Value, X(index), X(i));
                        return true;
                    }
                    
                }
                // I WAS HERE
                
            }
            //return true; THIS WAS THERE
            entry.TemporaryIndex = PrologRegisterTable.Instance.FindRegister();
            _generator.Emit(OpCodes.Put_Value, X(index), X(entry.TemporaryIndex));
            return true;
        }

        private int CompileGoalRecord(PrologCodeTerm term, int index)
        {
            ArrayList recs = new ArrayList();
            int nRecs = 0;
            ArrayList arguments = new ArrayList();

            if (term is PrologCodeNonEmptyList)
            {
                PrologCodeNonEmptyList NEList = (PrologCodeNonEmptyList)term;
                arguments.Add(NEList.Head);
                arguments.Add(NEList.Tail);
            }
            else if (term is PrologCodePredicate)
            {
                arguments = ((PrologCodePredicate)term).Arguments;
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                PrologCodeTerm r = (PrologCodeTerm)arguments[i];
                if (r is PrologCodeNonEmptyList || r is PrologCodePredicate)
                {
                    nRecs = recs.Add(CompileGoalRecord(r, -1));
                }
            }
            if (index == -1)
            {
                index = PrologRegisterTable.Instance.FindRegister();
            }
            if (term is PrologCodeNonEmptyList)
            {
                _generator.Emit(OpCodes.Put_List, X(index));
            }
            else if(term is PrologCodePredicate)
            {
                PrologCodePredicate s = (PrologCodePredicate)term;
                _generator.Emit(OpCodes.Put_Structure, s.Name + "/" + s.Arity, X(index));
            }
            nRecs = 0;
            for (int i = 0; i < arguments.Count; i++)
            {
                PrologCodeTerm t = (PrologCodeTerm)arguments[i];
                if (t is PrologCodeNilAtom || t is PrologCodeEmptyList)
                {
                    _generator.Emit(OpCodes.Set_Constant, "[]");
                }
                else if (t is PrologCodeAtom)
                {
                    if (t is PrologCodeConstantAtom)
                    {
                        _generator.Emit(OpCodes.Set_Constant, ((PrologCodeConstantAtom)t).Value);
                    }
                    else if (t is PrologCodeStringAtom)
                    {
                        _generator.Emit(OpCodes.Set_Constant, ((PrologCodeStringAtom)t).Value);
                    }
                    else if (t is PrologCodeIntegerAtom)
                    {

                        _generator.Emit(OpCodes.Set_Constant, ((PrologCodeIntegerAtom)t).Value.ToString());
                    
                    }
                    else if (t is PrologCodeFloatAtom)
                    {
                        _generator.Emit(OpCodes.Set_Constant, ((PrologCodeFloatAtom)t).Value.ToString());
                    
                    }
                }
                else if (t is PrologCodeVariable)
                {
                    /* Compile Goal record variable */
                    CompileGoalRecordVariable((PrologCodeVariable)t);
                }
                else if (t is PrologCodePredicate || t is PrologCodeNonEmptyList) 
                {
                    _generator.Emit(OpCodes.Set_Value, X((int)recs[nRecs]));
                    PrologRegisterTable.Instance.FreeRegister((int)recs[nRecs]);
                    nRecs++;
                }
            }
            return index;
        }

        private void CompileGoalRecordVariable(PrologCodeVariable var)
        {
            PrologVariableDictionaryEntry entry = _dictionary.GetVariable(var.Name);
            if (entry.IsReferenced)
            {
                /* Compile build value here ... */
                if (entry.IsTemporary)
                {
                    if (entry.IsGlobal)
                    {
                        _generator.Emit(OpCodes.Set_Value, X(entry.TemporaryIndex));
                    }
                    else
                    {
                        _generator.Emit(OpCodes.Set_Local_Value, X(entry.TemporaryIndex));
                        entry.IsGlobal = true;
                    }
                }
                else
                {
                    if (entry.IsGlobal)
                    {
                        if (entry.TemporaryIndex != -1)
                        {
                            _generator.Emit(OpCodes.Set_Value, X(entry.TemporaryIndex));
                        }
                        else
                        {
                            _generator.Emit(OpCodes.Set_Value, Y(entry.PermanentIndex));
                        }
                    }
                    else
                    {
                        if (entry.TemporaryIndex != -1)
                        {
                            _generator.Emit(OpCodes.Set_Local_Value, X(entry.TemporaryIndex));
                        }
                        else
                        {
                            _generator.Emit(OpCodes.Set_Local_Value, Y(entry.PermanentIndex));
                        }
                    }
                }
            }
            else
            {
                if (entry.IsTemporary)
                {
                    if (entry.Occurrences == 1)
                    {
                        _generator.Emit(OpCodes.Set_Void, "1");
                    }
                    else
                    {
                        _generator.Emit(OpCodes.Set_Variable, X(entry.TemporaryIndex));
                    }
                }
                else
                {
                    _generator.Emit(OpCodes.Set_Variable, Y(entry.PermanentIndex));
                }
                entry.IsGlobal = true;
            }
        }

        #endregion

        #region predicate compilation
        private void CompileCall(PrologCodeTerm p)
        {
            
            AMPredicateSet builtins = AMPredicateSet.Instance;
            PrologCodePredicate predicate = (PrologCodePredicate)p;

            if (builtins.IsBuiltin(predicate.Name, predicate.Arity))
            {
                CompileBuiltinPredicateCall(predicate);
            }
            else if (predicate.IsMethod)
            {
                CompileMethod(predicate);
            }
            else
            {
                CompilePrologPredicateCall(predicate);
            }
        }

        private void CompileBuiltinPredicateCall(PrologCodePredicate p)
        {
            AMPredicateSet pset = AMPredicateSet.Instance;
            _generator.EmitBCall((IAbstractMachinePredicate)pset.CreatePredicate(p.Name, p.Arity));

            if (_dictionary.InLastGoal)
            {
                if (_dictionary.GoalCount > 2)
                {
                    _generator.Emit(OpCodes.Deallocate);
                }
                // Emit 'proceed'
                _generator.EndProcedure();
            }
        }

        private void CompilePrologPredicateCall(PrologCodePredicate p)
        {
            if (_dictionary.InLastGoal)
            {
                if (_dictionary.GoalCount > 2)
                {
                    _generator.Emit(OpCodes.Deallocate);
                }
                _generator.EmitExecute(p.Name, p.Arity);
            }
            else
            {
                _generator.EmitCall(p.Name, p.Arity);
            }
        }
        #endregion
        #region Helper methods
        /* Initialize temporary goal variables in the dictionary */
        private void InitializeGoalTemporaryVariables(PrologCodeTerm goal)
        {
            /* Free all registers */
            PrologRegisterTable.Instance.FreeAllRegisters();

            int reg = 0;
            if (goal is PrologCodePredicate)
            {
                PrologCodePredicate g = (PrologCodePredicate)goal;

                if (g.Arity > 0)
                {
                    foreach (PrologCodeTerm var in g.Arguments)
                    {
                        if (var is PrologCodeVariable)
                        {
                            PrologVariableDictionaryEntry entry = _dictionary.GetVariable(((PrologCodeVariable)var).Name);
                            if (entry != null)
                            {
                                if (entry.IsTemporary && entry.TemporaryIndex == -1)
                                {
                                    _dictionary.AllocateTemporaryVariable(entry, reg);

                                }
                            }
                            reg++;
                        }
                    }
                }
            }
        }
        
      
        #endregion
    }
}