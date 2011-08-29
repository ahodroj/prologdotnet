//------------------------------------------------------------------------------
// <copyright file="PrologCompiler.cs" company="Axiom">
//     
//      Copyright (c) 2006 Axiom, Inc.  All rights reserved.
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
using System.IO;
using System.Reflection;
using System.Collections;
using System.CodeDom;
using System.CodeDom.Compiler;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;
using Axiom.Compiler.CodeObjectModel;

namespace Axiom.Compiler.Framework
{	
	public abstract class PrologCompiler :	PrologCodeGenerator, IPrologCompiler
	{

        PrologCompilerResults IPrologCompiler.CompileAbstractCodeFromUnit(PrologCompilerParameters p, PrologCodeUnit unit)
        {
            PrologCompilerResults results = new PrologCompilerResults();
            results.AbstractInstructions = new ArrayList();
            GenerateCodeFromUnit(unit, results.AbstractInstructions);

            /* patch predicates */
            //PatchPredicates(results.AbstractInstructions, GetPredicateAddresses(results.AbstractInstructions));

            /* save foreign methods */
            //results.ForeignMethods = GetForeignMethods(unit.Methods);

            /* save namespaces */
            results.Namespaces = unit.Namespaces;

            /* save assembly files */
            results.AssemblyFiles = unit.AssemblyFiles;

            /* return results */
            return results;
        }

        PrologCompilerResults IPrologCompiler.CompileAbstractCodeFromFile(PrologCompilerParameters p, string fileName)
        {
            PrologCompilerResults results = new PrologCompilerResults();
            PrologCodeParser parser = new PrologCodeParser();
            PrologCodeUnit unit = new PrologCodeUnit();
            try
            {
                StreamReader reader = new StreamReader(fileName);
                unit = parser.Parse(reader);
                
                /* Get errors after parsing */
                results.Errors = parser.Errors;
            }
            catch (FileNotFoundException)
            {
                results.Errors.Add(new PrologCompilerError("P0008", "Input file not found.", fileName, false, 0, 0));
                return results;
            }
            results.AbstractInstructions = new ArrayList();
            GenerateCodeFromUnit(unit, results.AbstractInstructions);
            
            /* patch predicates */
            //PatchPredicates(results.AbstractInstructions, GetPredicateAddresses(results.AbstractInstructions));

            /* Save foreign methods */
            //results.ForeignMethods = GetForeignMethods(unit.Methods);

            /* save namespaces */
            results.Namespaces = unit.Namespaces;

            /* save assembly files */
            results.AssemblyFiles = unit.AssemblyFiles;

            /* return results */
            return results;
            
        }
    
		PrologCompilerResults IPrologCompiler.CompileAssemblyFromUnit (PrologCompilerParameters options, PrologCodeUnit e)
		{
			return FromUnit(options, e); 
		}
		
		PrologCompilerResults IPrologCompiler.CompileAssemblyFromFile (PrologCompilerParameters options, string fileName)
		{
			return FromFile(options, fileName);    
		}
		
		protected virtual PrologCompilerResults FromUnit (PrologCompilerParameters options, PrologCodeUnit e)
		{
            ArrayList instructions = new ArrayList();
            PrologCompilerResults results = new PrologCompilerResults();

            /* Generate abstract machine instructions */
            GenerateCodeFromUnit(e, instructions);

            
            /* Determine assembly name and type to generate */
            if (options.GenerateExecutable)
            {
                results.CompiledAssembly = GenerateExecutableAssembly(options, instructions);
                
            }
            else
            {
               results.CompiledAssembly = GenerateDllAssembly(options, instructions, e);
            }
            return results;
        }

        private Assembly GenerateDllAssembly(PrologCompilerParameters compilerParameters, ArrayList instructions, PrologCodeUnit unit)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare namespace, default is Prolog.Assembly
            CodeNamespace plNamespace = new CodeNamespace("Prolog.Assembly");

            plNamespace.Imports.Add(new CodeNamespaceImport("System"));
            plNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
            plNamespace.Imports.Add(new CodeNamespaceImport("Axiom.Runtime"));

            compileUnit.Namespaces.Add(plNamespace);

            // Declare class type
            CodeTypeDeclaration classType = new CodeTypeDeclaration(unit.Class);
            plNamespace.Types.Add(classType);
            classType.TypeAttributes = TypeAttributes.Public;

            // Declare private members
            CodeMemberField machineField = new CodeMemberField(new CodeTypeReference("AbstractMachineState"), "machine");
            CodeMemberField moreField = new CodeMemberField(new CodeTypeReference("System.Boolean"), "_more");

            classType.Members.Add(machineField);
            classType.Members.Add(moreField);

            // Generate constructor method
            CodeConstructor cons = new CodeConstructor();
            cons.Attributes = MemberAttributes.Public;
            cons.Statements.Add(new CodeSnippetExpression("Init()"));
            classType.Members.Add(cons);

            // Generate the 'More' property
            CodeMemberProperty moreProperty = new CodeMemberProperty();
            moreProperty.Attributes = MemberAttributes.Public;
            moreProperty.Name = "More";
            moreProperty.HasGet = true;
            moreProperty.HasSet = false;
            moreProperty.Type = new CodeTypeReference("System.Boolean");
            string getStmt1 = "if (machine.Program.CurrentInstruction() == null || machine.Program.CurrentInstruction().Name().Equals(\"stop\")) {  _more = false; } ";
            string getStmt2 = "return !(machine.Program.CurrentInstruction() == null || machine.Program.CurrentInstruction().Name().Equals(\"stop\"));";
            moreProperty.GetStatements.Add(new CodeSnippetStatement(getStmt1));
            moreProperty.GetStatements.Add(new CodeSnippetStatement(getStmt2));
            classType.Members.Add(moreProperty);

            // Generate Redo() method
            CodeMemberMethod redoMethod = new CodeMemberMethod();
            redoMethod.Name = "Redo";
            redoMethod.Statements.Add(new CodeSnippetStatement("machine.Backtrack();"));
            redoMethod.Statements.Add(new CodeSnippetStatement("_more = true;"));
            redoMethod.Attributes = MemberAttributes.Public;
            classType.Members.Add(redoMethod);

            // Generate Init() method
            GenerateInitMethod(classType, instructions);

            // Generate method signatures
            GenerateMethodSignatures(classType, instructions);

            // Compile the file into a DLL
            CompilerParameters compparams = new CompilerParameters(new string[] { "mscorlib.dll", "Axiom.Runtime.dll" });
            
            compparams.GenerateInMemory = false;
            compparams.OutputAssembly = compilerParameters.OutputAssembly;
            compparams.TempFiles.KeepFiles = true;
            

            Microsoft.CSharp.CSharpCodeProvider csharp = new Microsoft.CSharp.CSharpCodeProvider();
            ICodeCompiler cscompiler = csharp.CreateCompiler();

            CompilerResults compresult = cscompiler.CompileAssemblyFromDom(compparams, compileUnit);

            if (compresult.Errors.Count > 0)
            {
                foreach(CompilerError err in compresult.Errors) 
                {
                    Console.WriteLine(err);
                }
                return null;
            }

            return compresult.CompiledAssembly;
        }

        private void GenerateMethodSignatures(CodeTypeDeclaration classType, ArrayList instructions)
        {
            Hashtable procedures = new Hashtable();
            // Get all predicate names
            foreach (AbstractInstruction i in instructions)
            {
                if (i.Name() == "procedure")
                {
                    ProcedureInstruction pi = (ProcedureInstruction)i;
                    if (!procedures.ContainsKey(pi.ProcedureName))
                    {
                        procedures.Add(pi.ProcedureName, pi);
                    }
                }
            }

            foreach (DictionaryEntry entry in procedures)
            {
                ProcedureInstruction pi = (ProcedureInstruction)entry.Value;
                GenerateMethod(classType, pi);
            }
        }

        private void GenerateMethod(CodeTypeDeclaration classType, ProcedureInstruction pi)
        {
            CodeMemberMethod method = new CodeMemberMethod();
            method.Name = pi.ProcedureName;
            method.ReturnType = new CodeTypeReference("System.Boolean");
            method.Attributes = MemberAttributes.Public;
            string objectStatement = "new object [] { ";
            for (int i = 0; i < pi.Arity; i++)
            {
                method.Parameters.Add(new CodeParameterDeclarationExpression("System.Object", "arg" + (i + 1)));
                objectStatement += "arg" + (i + 1);

                if (i == pi.Arity - 1)
                {
                    objectStatement += " }";
                }
                else
                {
                    objectStatement += ", ";
                }
            }

            method.Statements.Add(new CodeSnippetStatement("return machine.Call(\"" + pi.ProcedureName + "\", " + pi.Arity + ", " + objectStatement + ", _more);"));
            classType.Members.Add(method);
        }

        private void GenerateInitMethod(CodeTypeDeclaration classType, ArrayList instructions)
        {
            CodeMemberMethod initMethod = new CodeMemberMethod();
            initMethod.Attributes = MemberAttributes.Private;
            initMethod.Name = "Init";

            initMethod.Statements.Add(new CodeSnippetStatement("ArrayList program = new ArrayList();"));
            initMethod.Statements.Add(new CodeSnippetStatement("machine = new AbstractMachineState(new AMFactory());"));
            initMethod.Statements.Add(new CodeSnippetStatement("AMInstructionSet iset = new AMInstructionSet();"));
            initMethod.Statements.Add(new CodeSnippetStatement("_more = false;"));

            // generate instructions here...
            foreach (AbstractInstruction inst in instructions)
            {
                string statement = GetInstructionStatement(inst);
                initMethod.Statements.Add(new CodeSnippetStatement(statement));
            }


            initMethod.Statements.Add(new CodeSnippetStatement("machine.Initialize(program);"));

            classType.Members.Add(initMethod);
        }

        private string GetInstructionStatement(AbstractInstruction inst)
        {
            string statement = "program.Add(iset.CreateInstruction(";
            if (inst.Name() == "procedure")
            {
                ProcedureInstruction pi = (ProcedureInstruction)inst;
                statement += "\"procedure\", \"" + pi.ProcedureName + "\", \"" + pi.Arity + "\"";
            }
            else
            {
                if (inst._arguments == null || inst._arguments.Length == 0)
                {
                    statement += "\"" + inst.Name() + "\"";
                }
                else
                {
                    statement += "\"" + inst.Name() + "\", ";
                    for (int i = 0; i < inst._arguments.Length; i++)
                    {
                        statement += "\"" + inst._arguments[i] + "\"";
                        if (i != (inst._arguments.Length - 1))
                        {
                            statement += ", ";
                        }
                    }
                }
            }

            statement += "));";

            return statement;
        }

        private Assembly GenerateExecutableAssembly(PrologCompilerParameters compilerParameters, ArrayList instructions)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Declare namespace, default is Prolog.Assembly
            CodeNamespace plNamespace = new CodeNamespace("Prolog.Assembly");

            plNamespace.Imports.Add(new CodeNamespaceImport("System"));
            plNamespace.Imports.Add(new CodeNamespaceImport("System.Collections"));
            plNamespace.Imports.Add(new CodeNamespaceImport("Axiom.Runtime"));

            compileUnit.Namespaces.Add(plNamespace);

            // Declare class type
            CodeTypeDeclaration classType = new CodeTypeDeclaration("PrologApp");
            plNamespace.Types.Add(classType);
            classType.TypeAttributes = TypeAttributes.Public;

            // Declare private members
            CodeMemberField machineField = new CodeMemberField(new CodeTypeReference("AbstractMachineState"), "machine");

            CodeMemberField moreField = new CodeMemberField(new CodeTypeReference("System.Boolean"), "_more");

            classType.Members.Add(machineField);
            classType.Members.Add(moreField);

            // Generate constructor method
            CodeConstructor cons = new CodeConstructor();
            cons.Attributes = MemberAttributes.Public;
            cons.Statements.Add(new CodeSnippetExpression("Init()"));
            classType.Members.Add(cons);
            

            // Generate Init() method
            GenerateInitMethod(classType, instructions);

            // Generate main method
            CodeEntryPointMethod mainMethod = new CodeEntryPointMethod();
            mainMethod.Name = "Main";
            mainMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            mainMethod.Statements.Add(new CodeSnippetStatement("PrologApp app = new PrologApp();"));
            mainMethod.Statements.Add(new CodeSnippetStatement("app.Run();"));
            classType.Members.Add(mainMethod);


            CodeMemberMethod runMethod = new CodeMemberMethod();
            runMethod.Name = "Run";
            runMethod.Attributes = MemberAttributes.Public;
            runMethod.Statements.Add(new CodeSnippetStatement("machine.Call(\"main\");"));
            classType.Members.Add(runMethod);


            // Compile the file into a DLL
            CompilerParameters compparams = new CompilerParameters(new string[] { "mscorlib.dll", "Axiom.Runtime.dll" });

            compparams.GenerateInMemory = false;
            compparams.GenerateExecutable = true;
            compparams.OutputAssembly = compilerParameters.OutputAssembly;
            compparams.TempFiles.KeepFiles = true;

            Microsoft.CSharp.CSharpCodeProvider csharp = new Microsoft.CSharp.CSharpCodeProvider();
            ICodeCompiler cscompiler = csharp.CreateCompiler();

            CompilerResults compresult = cscompiler.CompileAssemblyFromDom(compparams, compileUnit);

            if (compresult.Errors.Count > 0)
            {
                foreach (CompilerError err in compresult.Errors)
                {
                    Console.WriteLine(err);
                }
                return null;
            }

            return compresult.CompiledAssembly;
        }

        

        protected virtual PrologCompilerResults FromFile (PrologCompilerParameters options, string fileName)
		{
            PrologCompilerResults results = new PrologCompilerResults();
            PrologCodeParser parser = new PrologCodeParser();
            PrologCodeUnit unit = new PrologCodeUnit();
            try
            {
                StreamReader reader = new StreamReader(fileName);
                unit = parser.Parse(reader);
            }
            catch (FileNotFoundException)
            {
                results.Errors.Add(new PrologCompilerError("P0008", "Input file not found.", fileName, false, 0, 0));
                return results;
            }
            return FromUnit(options, unit);
		}

	}
}