using System;
using System.Collections;
using System.Text;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime
{
    public class AMProgram : AbstractProgram
    {
        private Hashtable _labelOccurrence = new Hashtable();

        private int _numberOfArguments;
        public int NumberOfArguments
        {
            get { return _numberOfArguments; }
            set { _numberOfArguments = value; }
        }

        private ProgramNode _p;
        public ProgramNode P
        {
            get { return _p; }
            set { _p = value; }
        }

        private ProgramNode _cp;
        public ProgramNode CP
        {
            get { return _cp; }
            set { _cp = value; }
        }

        private ProgramNode _programTop;


        private ProgramNode _program;
        public ProgramNode Program
        {
            get { return _program; }
            set { _program = value; }
        }

        public void AddProgramNode(ProgramNode node)
        {
            if (_program == null)
            {
                _program = node;
                _programTop = _program;
                return;
            }
            _programTop.Next = node;
            _programTop = _programTop.Next;
        }

        public void AddInstruction(AbstractInstruction instruction)
        {
            if (_program == null)
            {
                _program = new ProgramNode(instruction);
                _programTop = _program;
                _p = _program;
                return;
            }
            _programTop.Next = new ProgramNode(instruction);
            _programTop = _programTop.Next;
        }

        public void Next()
        {
            _p = _p.Next;
        }

        public ProgramClause this[string procedureName]
        {
            get { return (ProgramClause)_labels[procedureName]; }
        }

        public override void Initialize(ArrayList program)
        {
            foreach (AbstractInstruction i in program)
            {
                if (i.Name() == "procedure")
                {
                    ProcedureInstruction p = (ProcedureInstruction)i;
                    ProgramClause pClause = new ProgramClause(p.ProcedureName, p.Arity);
                    AddLabel(pClause.Name + "/" + pClause.Arity, pClause);
                }
                else
                {
                    AddInstruction(i);
                }
            }

            _p = _program;
        }

        public override bool Stop()
        {
            return (_p == null || _p.Instruction.Name().Equals("halt"));
        }

        public override AbstractInstruction CurrentInstruction()
        {
            return _p.Instruction;
        }

        public bool IsDefined(string label)
        {
            return _labels.ContainsKey(label);
        }


        public void AddLabel(string label, ProgramClause procedure)
        {
            if (!_labels.ContainsKey(label))
            {
                _labels.Add(label, procedure);
                _labelOccurrence[label] = 1;
                AddProgramNode(procedure);
            }
            else
            {
                AddLabelAndPatchPredicates(label, procedure);
                int i = (int)_labelOccurrence[label];
                i++;
                _labelOccurrence[label] = i;
            }
        }


        private void AddLabelAndPatchPredicates(string label, ProgramClause procedure)
        {
            AMInstructionSet instructionSet = new AMInstructionSet();

            int occurrence = (int)_labelOccurrence[label];

            if (occurrence == 1)
            {
                ProgramClause p = (ProgramClause)_labels[label];
                string nextLabelName = p.Name + "%" + occurrence + "/" + p.Arity;

                p.Instruction = instructionSet.CreateInstruction("try_me_else", nextLabelName);

                ProgramClause newClause = new ProgramClause(nextLabelName, p.Arity, instructionSet.CreateInstruction("trust_me"));

                p.NextPredicate = newClause;

                AddProgramNode(newClause);

                _labels[nextLabelName] = newClause;
            }
            else
            {
                ProgramClause pc = (ProgramClause)_labels[label];
                string nextLabelName = pc.Name + "%" + occurrence + "/" + pc.Arity;

                ProgramClause lastLabel = this[pc.Name + "%" + (occurrence - 1) + "/" + pc.Arity];

                lastLabel.Instruction = instructionSet.CreateInstruction("retry_me_else", nextLabelName);

                ProgramClause newClause = new ProgramClause(nextLabelName, pc.Arity, instructionSet.CreateInstruction("trust_me"));

                lastLabel.NextPredicate = newClause;

                AddProgramNode(newClause);

                _labels[nextLabelName] = newClause;
            }
        }

        private Hashtable _labels = new Hashtable();

        public void AssertFirst(string predicateName, int arity, ArrayList code)
        {
            AMInstructionSet iset = new AMInstructionSet();

            if (!_labels.ContainsKey(predicateName + "/" + arity))
            {
                AddLabel(predicateName + "/" + arity, new ProgramClause(predicateName, arity));
                _labelOccurrence[predicateName + "/" + arity] = 1;
                // add instructions
                

                return;
            }

            string pLabel = predicateName + "/" + arity;

            int occurrence = (int)_labelOccurrence[predicateName + "/" + arity];

            if (occurrence == 1)
            {
                ProgramClause oldPredicate = (ProgramClause)_labels[pLabel];
                oldPredicate.Instruction = iset.CreateInstruction("trust_me");

                string nextLabel = predicateName + "%" + occurrence + "/" + arity;

                oldPredicate.Name = nextLabel;
                _labels[nextLabel] = oldPredicate;


                ProgramClause newFirst = new ProgramClause(predicateName, arity, iset.CreateInstruction("try_me_else", nextLabel));
                newFirst.NextPredicate = oldPredicate;

                _labels[pLabel] = newFirst;

                occurrence++;

                _labelOccurrence[pLabel] = occurrence;

                AddProgramNode(newFirst);

                foreach (AbstractInstruction inst in code)
                {
                    AddInstruction(inst);
                }
            }
            else
            {
                ProgramClause oldFirst = (ProgramClause)_labels[pLabel];

                ProgramClause newFirst = new ProgramClause(predicateName, arity, iset.CreateInstruction("try_me_else", predicateName + "%1/" + arity));
                newFirst.NextPredicate = oldFirst;

                oldFirst.Name = predicateName + "%1/" + arity;
                oldFirst.Instruction = iset.CreateInstruction("retry_me_else", predicateName + "%2/" + arity);

                _labels[pLabel] = newFirst;

                AddProgramNode(newFirst);

                foreach (AbstractInstruction inst in code)
                {
                    AddInstruction(inst);
                }
                occurrence++;

                _labelOccurrence[pLabel] = occurrence;

                PatchPredicates(predicateName, arity, oldFirst);
                
            }
        }

        private void PatchPredicates(string predicateName, int arity, ProgramClause oldFirst)
        {
            AMInstructionSet iset = new AMInstructionSet();

            int i = 1;
            ProgramClause clause = null;
            for (clause = oldFirst; clause.Instruction.Name() != "trust_me"; clause = clause.NextPredicate)
            {
                clause.Name = predicateName + "%" + i + "/" + arity;
                if (clause.Instruction.Name() != "try_me_else")
                {
                    clause.Instruction = iset.CreateInstruction("retry_me_else", predicateName + "%" + (i + 1) + "/" + arity);
                }
                _labels[predicateName + "%" + i + "/" + arity] = clause;
                i++;
            }
            // patch the last predicate also
            clause.Name = predicateName + "%" + i + "/" + arity;
            _labels[predicateName + "%" + i + "/" + arity] = clause;
        }

        public void AssertLast(string predicateName, int arity, ArrayList code)
        {
            AddLabel(predicateName + "/" + arity, new ProgramClause(predicateName, arity));
            
            foreach (AbstractInstruction inst in code)
            {
                AddInstruction(inst);
            }
        }
    }
}
