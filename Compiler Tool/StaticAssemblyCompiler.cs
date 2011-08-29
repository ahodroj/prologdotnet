
using System;
using System.Collections;
using ILMerging;

namespace Axiom.Compiler.Framework
{
    /// <summary>
    /// Description of AssemblyMerger.
    /// </summary>
    public class StaticAssemblyCompiler
    {
        private string _outputFile;

        private string _inputFile;

        private ArrayList _inputAssemblies;

        public StaticAssemblyCompiler(string outputFile, string inputFile, ArrayList inputAssemblies)
        {
            _outputFile = outputFile;
            _inputFile = inputFile;
            _inputAssemblies = inputAssemblies;
        }

        public void LinkLibrary()
        {
            ILMerge merger = new ILMerge();

            merger.OutputFile = _outputFile;
            _inputAssemblies.Insert(0, _inputFile);
            merger.SetInputAssemblies((string[])_inputAssemblies.ToArray());
            merger.SetSearchDirectories(new string[] { "..\\runtime", "..\\library" });
            merger.TargetKind = ILMerge.Kind.Dll;
            merger.Merge();
        }

        public void LinkExecutable()
        {
            ILMerge merger = new ILMerge();

            merger.OutputFile = _outputFile;
            _inputAssemblies.Insert(0, _inputFile);
            merger.SetInputAssemblies((string[])_inputAssemblies.ToArray());
            merger.SetSearchDirectories(new string[] { "..\\runtime", "..\\library" });
            merger.TargetKind = ILMerge.Kind.Exe;
            merger.Merge();
        }

    }
}
