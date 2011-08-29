using System;
using System.IO;	
using System.Collections;
using Axiom.Runtime.Instructions;


namespace Axiom.Runtime
{
	
	public class AbstractMachineState
	{
        // passed variables: used for Call() method
        private ArrayList _passedVariables = new ArrayList();

        private Choicepoint _b;
        public Choicepoint B
        {
            get { return _b; }
            set { _b = value; }
        }

        private Choicepoint _b0;
        public Choicepoint B0
        {
            get { return _b0; }
            set { _b0 = value; }
        }

        private EnvironmentFrame _e;
        public EnvironmentFrame E
        {
            get { return _e; }
            set { _e = value; }
        }

        private HeapNode _s;
        public HeapNode S
        {
            get { return _s; }
            set {   _s = value; }
        }

        private bool _fail = false;
        public bool Fail
        {
            get { return _fail; }
            set { _fail = value; }
        }
        private bool _writeMode = false;
        public bool IsWriteMode
        {
            get { return _writeMode; }
            set { _writeMode = value; }
        }

        public bool IsReadMode
        {
            get { return !_writeMode; }
            set { _writeMode = !(value); }
        }

        private AbstractDataArea _dataArea;
        public AbstractDataArea DataArea
        {
            get { return _dataArea; }
            set { _dataArea = value; }
        }

        private AbstractAssemblyCache _ac;
        public AbstractAssemblyCache AssemblyCache
        {
            get { return _ac; }
        }

        private ArrayList _registers;
        
        private AbstractProgram _program;
        public AbstractProgram Program
        {
            get { return _program; }
            set { _program = value; }
        }

        private AbstractTrail _trail;
        public AbstractTrail Trail
        {
            get { return _trail; }
            set { _trail = value; }
        }

        public AbstractMachineState(AbstractMachineFactory factory)
        {
            _program = factory.CreateProgram();
            _dataArea = factory.CreateDataArea();
            _trail = factory.CreateTrail();
            _ac = factory.CreateCache();
            
        }

        public void Initialize(ArrayList instructions)
        {
            _program.Initialize(instructions);
            _dataArea.Initialize(this);
           
            _registers = new ArrayList();

            // Initialize all registers
            for (int i = 0; i < 23; i++)
            {
                _registers.Add(new AbstractTerm());
            }

        }

        public void Initialize(ArrayList instructions, ArrayList namespaces, ArrayList assemblyFiles)
        {
            _program.Initialize(instructions);
            _dataArea.Initialize(this);
            /* Initialize local assembly cache */
            _ac.Init();

            /* load the required namespaces */
            _ac.Load(namespaces, assemblyFiles);


            _registers = new ArrayList();

            // Initialize all registers
            for (int i = 0; i < 23; i++)
            {
                _registers.Add(new AbstractTerm());
            }

        }

        public bool Stop()
		{
            return _program.Stop() && _dataArea.Stop();
		}

		public void Transition()
		{
            while (!Stop())
            {
                AbstractInstruction next = _program.CurrentInstruction();
                AMHeap heap = (AMHeap)_dataArea;

                next.Execute(this);
            }
		}

        public void Step()
        {
            if (!Stop())
            {
                _program.CurrentInstruction().Execute(this);
            }
        }
 
        public void Backtrack()
        {
            AMProgram program = (AMProgram)_program;
            if (_b == null)
            {
                program.P = new ProgramNode(new HaltInstruction());
            }
            else
            {
                _fail = false;
                program.P = _b.NextClause;
            }
        }

        
        public object this[string registerName]
        {
            get
            {
                return GetRegisterValue(registerName);
            }
        }

        private object GetRegisterValue(string registerName)
        {
            switch (registerName[0])
            {
                case 'Y':
                    return GetPermanentRegister(registerName);
                case 'X':
                    return GetDataRegister(registerName);
                case 'H':
                    AMHeap heap = (AMHeap)_dataArea;
                    return heap.H;
               
            }
            return null;
           
        }

        private object GetPermanentRegister(string registerName)
        {
            return _e[registerName];
        }

        private AbstractTerm GetDataRegister(string registerName)
        {
            int cnt = 0;
            int index = 0;

            while (++cnt < registerName.Length)
            {
                index = index * 10 + (registerName[cnt] - '0');
            }
            // TODO: Get Y register
            //if (name[0] == 'Y')
            //{
            //    return (Variable)heap[prog.E + 4 + index];
            //}
            cnt = _registers.Count;
            while (cnt++ < index + 1)
                _registers.Add(new AbstractTerm());
            return (AbstractTerm)_registers[index];
        }

        // Foreign Predicate Table
        private Hashtable _foreignPredicateTable = new Hashtable();


        public AMForeignPredicate GetForeignPredicate(string name)
        {
            return (AMForeignPredicate)_foreignPredicateTable[name];
        }


        public void AddForeignPredicate(AMForeignPredicate fp)
        {
            if (!_foreignPredicateTable.ContainsKey(fp.PredicateName))
            {
                _foreignPredicateTable.Add(fp.PredicateName, fp);
            }
        }

       
        public bool Call(string predicateName, int arity, object[] args)
        {

            AMProgram program = (AMProgram)_program;
            AMHeap heap = (AMHeap)_dataArea;
            AMInstructionSet iset = new AMInstructionSet();

            if (!program.IsDefined(predicateName + "/" + arity))
            {
                return false;
            }
            ArrayList a = new ArrayList();

            ProgramNode entryPoint = null;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].GetType().ToString())
                {
                    case "System.String":
                    case "System.Int32":
                    case "System.Boolean":
                        if (entryPoint == null)
                        {
                            entryPoint = new ProgramNode(iset.CreateInstruction("put_constant", args[i].ToString(), "X" + i.ToString()));
                            program.AddProgramNode(entryPoint);
                        }
                        else
                        {
                            program.AddInstruction(iset.CreateInstruction("put_constant", args[i].ToString(), "X" + i.ToString()));
                        }
                        break;
                    case "Axiom.Runtime.AbstractTerm":
                        a.Add(i);
                        if (entryPoint == null)
                        {
                            entryPoint = new ProgramNode(iset.CreateInstruction("put_variable", "X" + args[i].ToString(), "X" + i.ToString()));
                            program.AddProgramNode(entryPoint);
                        }
                        else
                        {
                            program.AddInstruction(iset.CreateInstruction("put_variable", "X" + args[i].ToString(), "X" + args[i].ToString()));
                        }
                        break;
                }
            }

            // Add the call instruction
            program.AddInstruction(iset.CreateInstruction("call", predicateName, arity.ToString()));
            
            // Add the halt insturction
            program.AddInstruction(iset.CreateInstruction("halt"));

            program.P = entryPoint;

            // Execute the program
            Transition();

            foreach (int argumentNumber in a)
            {
                AbstractTerm var = (AbstractTerm)args[argumentNumber];
                var.Assign((AbstractTerm)this["X" + argumentNumber.ToString()]);
            }

            return !_fail;
        }

        /// <summary>
        /// Call a predicate that takes no arguments
        /// </summary>
        /// <param name="predicateName">predicate name only.</param>
        /// <returns>success or failure</returns>
        public bool Call(string predicateName)
        {

            AMProgram program = (AMProgram)_program;
            
            AMInstructionSet iset = new AMInstructionSet();

            if (!program.IsDefined(predicateName + "/0"))
            {
                return false;
            }
            
            // Add the call instruction
            program.P = new ProgramNode(iset.CreateInstruction("call", predicateName, "0"));

            program.AddProgramNode(program.P);

            // Add the halt insturction
            program.AddInstruction(iset.CreateInstruction("halt"));

            // Execute the program
            Transition();

            return !_fail;
        }

        public bool Call(string predicatename, int arity, object[] args, bool more)
        {
            AMProgram program = (AMProgram)_program;
            AMHeap heap = (AMHeap)_dataArea;
            AMInstructionSet iset = new AMInstructionSet();

            // argument indexes
            ArrayList argumentIndexes = new ArrayList();

            if (!more)
            {
                _passedVariables = new ArrayList();
            }

            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != null)
                {
                    switch (args[i].GetType().ToString())
                    {
                        case "System.String":
                        case "System.Int32":
                        case "System.Boolean":
                            if (!more)
                            {
                                AbstractTerm var = new ConstantTerm(args[i].ToString());
                                heap.Push(var);
                                _passedVariables.Add(-1);
                                AbstractTerm Xn = (AbstractTerm)this["X" + i];
                                Xn.Assign(var);
                            }
                            break;
                        case "Axiom.Runtime.AbstractTerm":
                            if (!more)
                            {
                                AbstractTerm heapVariable = new AbstractTerm();
                                heap.Push(heapVariable);
                                _passedVariables.Add(heapVariable);
                                AbstractTerm Xn = (AbstractTerm)this["X" + i];
                                Xn.Assign(heapVariable);
                            }
                            break;
                    }
                }
            }

            if (!more)
            {
                program.P = new ProgramNode(iset.CreateInstruction("call", predicatename, arity.ToString()));
                program.AddProgramNode(program.P);
                program.AddInstruction(iset.CreateInstruction("halt"));
            }
            
            // Execute the program
            Transition();

            for (int i = 0; i < _passedVariables.Count; i++)
            {
                if (!(_passedVariables[i] is int))
                {
                    AbstractTerm v = (AbstractTerm)_passedVariables[i];
                    AbstractTerm argumentVariable = (AbstractTerm)args[i];
                    argumentVariable.Assign(v.Dereference());
                }
            }

            return !_fail;
        }
    }
}

