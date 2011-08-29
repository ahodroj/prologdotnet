using System;
using System.Collections;
using System.IO;
using Axiom.Compiler.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;
using System.Xml;

namespace Prolog.CT
{
	/// <summary>
	/// Summary description for CompilerTool.
	/// </summary>
	public class CompilerTool
	{
		private CommandLineArguments cmdOptions;
		private CompilerView compiler = null;
		private Hashtable errors = new Hashtable();
		private ArrayList cliErrors = new ArrayList();
        private bool staticAssembly = false;

		public CompilerTool()
		{
			errors["CT1008"] = "No input file specified.";
			errors["CT1009"] = "Unrecognized command-line option: ";
			errors["CT1010"] = "Source file could not be found: ";
			errors["CT1011"] = "Invalid target type for /target: must specify 'console' or 'gui'.";
		}
	
		public static void Main(string [] args) 
		{
            CompilerTool ct = new CompilerTool();
            ct.ParseArguments(args);
            ct.Run();
     
		}

		public void Errors() 
		{
			foreach(string e in cliErrors) 
			{
				Console.WriteLine(e);
			}
		}
		public void Error(string errorKey, string message) 
		{
			cliErrors.Add("error " + errorKey + ": " + errors[errorKey] + message);
		}
		
        /* Run the Compiler Tool application */
		public void Run() 
		{
			if(cmdOptions.NoArgs()) 
			{
				PrintLogo();
				this.compiler = new InteractiveCompiler();
				return;
			}
			if(cmdOptions.NoOptions()) 
			{
				PrintLogo();
				this.compiler = new InteractiveCompiler(cmdOptions["source"]);
				return;
			}
			this.compiler = new CLICompiler();
			/* Check command line options */
			if(cmdOptions["help"] != null) 
			{
				Help();
				return;
			}
			if(cmdOptions["version"] != null) 
			{
				PrintLogo();
				return;
			}
			if(cmdOptions["source"] == null) 
			{
				Error("CT1008", null);
			}

            if (cmdOptions["static"] != null)
            {
                staticAssembly = true;
            }

			if(cmdOptions["listing"] != null) 
			{
				/* set the listing generator */
				string outputFile = cmdOptions["source"] + ".txt";
				if(cmdOptions["output"] != null) 
				{
					outputFile = cmdOptions["output"];
				}
				//compiler.SetGenerator(new ListingGenerator(outputFile));
			}
			if(cmdOptions["target"] != null) 
			{
				switch(cmdOptions["target"]) 
				{
					case "exe":
                        
						string outputFile = cmdOptions["source"];
						if(cmdOptions["output"] != null) 
						{
							outputFile = cmdOptions["output"];
						}
                        PrologCodeProvider provider = new PrologCodeProvider();
                        IPrologCompiler compiler = provider.CreateCompiler();
                        PrintLogo();
                        PrologCompilerParameters parameters = new PrologCompilerParameters();
                        parameters.GenerateExecutable = true;
                        parameters.OutputAssembly = outputFile.Replace(".pro", ".exe");
                        PrologCompilerResults cr = compiler.CompileAssemblyFromFile(parameters, outputFile);
                        
                        if (cr.Errors.Count > 0)
                        {
                            foreach (PrologCompilerError e in cr.Errors)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Successfully compiled " + outputFile);
                        }
                        // check if you need to compile into static EXE
                        if (staticAssembly)
                        {
                            ArrayList inputAssemblies = new ArrayList();
                            // Add Axiom.Runtime.dll
                            inputAssemblies.Add("Axiom.Runtime.dll");
                            

                            // Add remaining library assemblies
                            foreach (string file in Directory.GetFiles("..\\library"))
                            {
                                inputAssemblies.Add(file);
                            }

                            StaticAssemblyCompiler assc = new StaticAssemblyCompiler(parameters.OutputAssembly, parameters.OutputAssembly, inputAssemblies);
                            assc.LinkExecutable();
                        }
                        return;
					case "dll":
						string output = cmdOptions["source"];
						if(cmdOptions["output"] != null) 
						{
							output = cmdOptions["output"];
						}
						//PrologAssemblyCompiler asmc = new PrologAssemblyCompiler();
                        PrologCodeProvider prov = new PrologCodeProvider();
                        IPrologCompiler comp = prov.CreateCompiler();
                        PrintLogo();
                        PrologCompilerParameters par = new PrologCompilerParameters();
                        par.GenerateExecutable = false;
                        par.OutputAssembly = output.Replace(".pro", ".dll");
                        PrologCompilerResults r = comp.CompileAssemblyFromFile(par, output);
                        if (r.Errors.Count > 0)
                        {
                            foreach (PrologCompilerError e in r.Errors)
                            {
                                Console.WriteLine(e);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Successfully compiled " + output);
                        }
                        // check if you need to compile into static DLL
                        if (staticAssembly)
                        {
                            ArrayList inputAssemblies = new ArrayList();

                            // Add Axiom.Runtime.dll
                            inputAssemblies.Add("Axiom.Runtime.dll");


                            // Add remaining library assemblies
                            foreach (string file in Directory.GetFiles("..\\library"))
                            {
                                inputAssemblies.Add(file);
                            }

                            StaticAssemblyCompiler assc = new StaticAssemblyCompiler(par.OutputAssembly, par.OutputAssembly, inputAssemblies);
                            assc.LinkLibrary();
                        }
						return;
                    case "xml":
                        string outputXml = cmdOptions["source"];
                        if (cmdOptions["output"] != null)
                        {
                            outputXml = cmdOptions["output"];
                        }
                        PrologCodeProvider providerXml = new PrologCodeProvider();
                        IPrologCompiler compilerXml = providerXml.CreateCompiler();
                        PrologCompilerParameters parametersXml = new PrologCompilerParameters();
                        PrologCompilerResults results = compilerXml.CompileAbstractCodeFromFile(parametersXml, outputXml);
                        if (results.Errors.Count > 0)
                        {
                            foreach (PrologCompilerError err in results.Errors)
                            {
                                Console.WriteLine(err);
                            }
                            return;
                        }

                        string plFilename = outputXml;
                        string xmlFilename = plFilename.Replace(".pro", ".xml");
                        
                        GenerateXmlFile(xmlFilename, results.AssemblyFiles, results.AbstractInstructions);
                        return;
					default:
						Error("CT1011", null);
						break;
				}
			}
			/* debug information */
			// deferred to a future release.

			/* treat warnings as errors */
			// deferred to a future release.

			/* silent compilation */
			// deferred to a future release.
			

			if(cmdOptions["nologo"] == null) 
			{
				PrintLogo();
			}

			if(cliErrors.Count != 0) 
			{
				Errors();
				return;
			}

			/* Compile */
			//this.compiler.Compile(File.OpenText(cmdOptions["source"]));
		}
		public void ParseArguments(string [] args) 
		{
			this.cmdOptions = new CommandLineArguments(args);
			
			foreach(string a in args) 
			{
				if(a.IndexOf("/") == -1) 
				{
					cmdOptions.Add("source", a);
					break;
				}
			}	
		}

		/* Prolog compiler tool options
		 * 
		 * source code file listing
		 * hide product logo
		 * silent compilation
		 * print product logo
		 * specify output file
		 * create a console application
		 * create a windows application
		 * interactive console interface
		 * debug information
		 * treat warnings as errors
		 * set warning level
		 * display help 
		 */
		public void Help() 
		{
			PrintLogo();
			Console.WriteLine("\n\t-- Compiler Tool Options --\n");
			//Console.WriteLine("/listing\t\tProduce a source code file listing");
			Console.WriteLine("/nologo\t\t\tHides product logo.");
			Console.WriteLine("/version\t\tPrints product version.");
			Console.WriteLine("/silent\t\t\tSilent compilation.");
			Console.WriteLine("/output:<name>\t\tSpecify output file name.");
			Console.WriteLine("/target:exe\t\tBuild a console executable.");
			Console.WriteLine("/target:dll\t\tBuild a .NET Assembly.");
            Console.WriteLine("/target:xml\t\tGenerate an intermediate language XML file.");
            Console.WriteLine("/static\t\t\tCreate a static assembly.");
			//Console.WriteLine("/debug\t\t\tPrint debug information.");
			//Console.WriteLine("/warnaserrors\t\tTreat warnings as errors.");
			Console.WriteLine("/version\t\tPrint product logo.");
			Console.WriteLine("/help\t\t\tDisplay this help screen.");
			//Console.WriteLine("\nif no arguments are given the compiler will launch in interactive mode.");
		}
		
		public void PrintLogo() 
		{
			Console.WriteLine("Prolog.NET Compiler version {0}", AxiomReleaseInformation.CompilerVersion);
			Console.WriteLine("for {0} version {1}", AxiomReleaseInformation.Product, AxiomReleaseInformation.Release);
			Console.WriteLine("{0} {1}.", AxiomReleaseInformation.Copyright, AxiomReleaseInformation.Company);
			Console.WriteLine("");
		}

        private static void GenerateXmlFile(string xmlFilename, ArrayList assemblyFiles, ArrayList arrayList)
        {
            XmlTextWriter xmltw = new XmlTextWriter(xmlFilename, null);

            xmltw.Formatting = Formatting.Indented;


            xmltw.WriteStartDocument();

            xmltw.WriteComment(" This file was automatically generated by axiomc ");
            xmltw.WriteComment(" Source: " + xmlFilename.Replace(".xml", ".pro") + ",    Date: " + DateTime.Now + " ");



            xmltw.WriteStartElement("AMProgram");

            /* write namespaces and assembly files here */
            if(assemblyFiles.Count > 0) {
                xmltw.WriteStartElement("AssemblyFiles");
                foreach (string asmFile in assemblyFiles)
                {
                    xmltw.WriteStartElement("LoadAssembly");
                    xmltw.WriteString(asmFile);
                    xmltw.WriteEndElement();
                }
                xmltw.WriteEndElement();
            }

            for (int i = 0; i < arrayList.Count; i++)
            {
                AbstractInstruction inst = (AbstractInstruction)arrayList[i];
                if (inst.Name() == "procedure")
                {
                    ProcedureInstruction p = (ProcedureInstruction)inst;
                    xmltw.WriteStartElement("Predicate");
                    xmltw.WriteAttributeString("name", p.ProcedureName + "/" + p.Arity);
                    for (int j = i + 1; j < arrayList.Count; j++)
                    {
                        AbstractInstruction cinst = (AbstractInstruction)arrayList[j];
                        WriteXmlInstruction(xmltw, cinst);
                        if (cinst.Name() == "proceed" || cinst.Name() == "execute")
                        {
                            i = j;
                            break;
                        }
                    }
                    xmltw.WriteEndElement();

                }
                else
                {
                    WriteXmlInstruction(xmltw, inst);
                }
            }


            xmltw.WriteEndElement();
            xmltw.WriteEndDocument();
            xmltw.Flush();
            xmltw.Close();
        }

        private static void WriteXmlInstruction(XmlTextWriter xmltw, AbstractInstruction cinst)
        {
            switch (cinst.Name())
            {
                case "call":
                case "execute":
                case "bcall":
                    xmltw.WriteStartElement(cinst.Name());
                    xmltw.WriteAttributeString("name", cinst.ToString().Split(new char[] { ' ' })[1]);
                    xmltw.WriteEndElement();
                    break;
                case "fcall":
                    FCallInstruction fcinst = (FCallInstruction)cinst;
                    xmltw.WriteStartElement(cinst.Name());
                    xmltw.WriteAttributeString("assembly", fcinst._assemblyName);
                    xmltw.WriteAttributeString("class", fcinst._classType);
                    xmltw.WriteAttributeString("method", fcinst._methodName);
                    xmltw.WriteAttributeString("predicate", fcinst._predicateName);
                    break;
                default:
                    WriteXmlInstructionArguments(xmltw, cinst);
                    break;
            }
        }

        private static void WriteXmlInstructionArguments(XmlTextWriter xmltw, AbstractInstruction cinst)
        {
            switch (cinst.NumberOfArguments())
            {
                case 0:
                    xmltw.WriteStartElement(cinst.Name());
                    xmltw.WriteEndElement();
                    break;
                case 1:
                    xmltw.WriteStartElement(cinst.Name());
                    xmltw.WriteAttributeString("arg1", cinst.ToString().Split(new char[] { ' ' })[1]);
                    xmltw.WriteEndElement();
                    break;
                case 2:
                    xmltw.WriteStartElement(cinst.Name());
                    if (cinst.Name() == "put_structure")
                    {
                        string args = cinst.ToString().Split(new char[] { ' ' })[1];
                        xmltw.WriteAttributeString("arg1", args.Split(new char[] { ',' })[0]);
                        xmltw.WriteAttributeString("arg2", args.Split(new char[] { ',' })[1]);
                        xmltw.WriteEndElement();
                    }
                    else
                    {
                        if (cinst.ToString().IndexOf('"') != -1)
                        {
                            // put_constant "Hello, World", X0
                            string instStr = cinst.ToString();
                            string arg1 = instStr.Substring(instStr.IndexOf('"'), instStr.LastIndexOf('"') - instStr.IndexOf('"') + 1);
                            string arg2 = instStr.Substring(instStr.LastIndexOf('"') + 2);
                            xmltw.WriteAttributeString("arg1", arg1);
                            xmltw.WriteAttributeString("arg2", arg2);
                            xmltw.WriteEndElement();
                        }
                        else if (cinst.ToString().IndexOf('\'') != -1)
                        {
                            // put_constant 'Hello, World', X0
                            string instStr = cinst.ToString();
                            string arg1 = instStr.Substring(instStr.IndexOf('\''), instStr.LastIndexOf('\'') - instStr.IndexOf('\'') + 1);
                            string arg2 = instStr.Substring(instStr.LastIndexOf('\'') + 2);
                            xmltw.WriteAttributeString("arg1", arg1);
                            xmltw.WriteAttributeString("arg2", arg2);
                            xmltw.WriteEndElement();
                        }
                        else
                        {
                            string args = cinst.ToString().Split(new char[] { ' ' })[1];
                            xmltw.WriteAttributeString("arg1", args.Split(new char[] { ',' })[0]);
                            xmltw.WriteAttributeString("arg2", args.Split(new char[] { ',' })[1]);
                            xmltw.WriteEndElement();
                        }
                    }
                    break;
            }
        }

	}
}
