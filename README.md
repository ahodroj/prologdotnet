# Overview

Prolog is a logic programming language associated with artificial intelligence and computational linguistics. Prolog has its roots in first-order logic, a formal logic, and unlike many other programming languages, Prolog is intended primarily as a declarative programming language: the program logic is expressed in terms of relations, represented as facts and rules. A computation is initiated by running a query over these relations.[4]

Prolog.NET provides an implementation of Prolog programming language on the .NET Framework. It includes an enhanced compiler with language extensions and a code generator targeting Microsoftâ€™s Intermediate Language (MSIL).

# Why Prolog.NET? 

The purpose of this project is to provide seamless integration between .NET and Prolog by compiling Prolog code into reusable .NET objects that leverage the rich feature set provided by the .NET framework. This makes it easy to develop applications that combine the powerful features of both imperative and logic programming paradigms, as well as use logic programming as a meta-language to reason about object-oriented programs.

Prolog.NET was developed by Ali M. Hodroj as a senior capstone project at the Oregon Institute of Technology.

# Language Design / Compiler Architecture

The Prolog.NET system consists of a Prolog compiler and a runtime environment. The compiler (prologc.exe) is responsible for compiling Prolog programs into either abstract machine code to be executed by the runtime environment or .NET intermediate language executed directly by the .NET Common Language Runtime. The runtime environment is a .NET implementation of a Warren Abstract Machine with an extended instruction set that facilitates interoperability between Prolog and .NET. Upon installing Prolog.NET, the Compiler libraries as well as the runtime environment will be automatically installed in the Global Assembly Cache (GAC) by the installer.

There are three output formats that a Prolog program can be compiled into using the Prolog.NET compiler: Executables (.exe), Class libraries (.dll), or Abstract Machine files (.xml). Both executables and class libraries will require the runtime environment installed in the global assembly cache in order to execute. However, the compiler also uses the ILMerge utility from Microsoft Research to merge the runtime environment library with the executable or DLL so that they can be run or linked on a system that doesn’t have the runtime environment installed. Abstract machine files are XML files that represent the Prolog programs in Warren Abstract Machine format. These files can be executed using the prolog.exe program, which acts as the main driver for the runtime environment interpreter.

# Compiling Prolog Programs into .NET assemblies

There are two types of assemblies that can be generated using the compiler: dynamically-linked libraries and executables. The .NET assemblies generated will require the runtime library installed in the global assembly cache (GAC). However, the compiler can also merge the generated assembly with the Prolog runtime library to implement a completely independent static executable. The type of assembly target can be specified via the /target switch in the prologc executable.

In order to compile a Prolog program into a DLL assembly the /target:dll switch must be specified with the compiler. Every Prolog source code file is compiled into one .NET class. The class name has to be specified via the class/1 directive in the source file. Once compiled, every Prolog predicate is compiled into a public class member method. Since predicates can either be true or false, we set the return types of the compiled methods to be System.Boolean. The examples below show how a compiled Prolog program would look like if reverse-engineered in C#:

  :- class('MyMath').

  factorial(0,1).
  factorial(N,F) :- N > 0, M is N - 1, factorial(M,Fm), F is N * Fm.

  fib(0,1).
  fib(1,1).
  fib(N,F) :- N > 1, N1 is N - 1, N2 is N - 2, fib(N1,F1), fib(N2,F2), F is F1+F2
