# Overview

Prolog is a logic programming language associated with artificial intelligence and computational linguistics. Prolog has its roots in first-order logic, a formal logic, and unlike many other programming languages, Prolog is intended primarily as a declarative programming language: the program logic is expressed in terms of relations, represented as facts and rules. A computation is initiated by running a query over these relations.[4]

Prolog.NET provides an implementation of Prolog programming language on the .NET Framework. It includes an enhanced compiler with language extensions and a code generator targeting Microsoftâ€™s Intermediate Language (MSIL).

# Why Prolog.NET? 

The purpose of this project is to provide seamless integration between .NET and Prolog by compiling Prolog code into reusable .NET objects that leverage the rich feature set provided by the .NET framework. This makes it easy to develop applications that combine the powerful features of both imperative and logic programming paradigms, as well as use logic programming as a meta-language to reason about object-oriented programs.

Prolog.NET was developed by Ali M. Hodroj as a senior capstone project at the Oregon Institute of Technology.


# Compiling Prolog Programs into .NET assemblies

There are two types of assemblies that can be generated using the compiler: dynamically-linked libraries and executables. The .NET assemblies generated will require the runtime library installed in the global assembly cache (GAC). However, the compiler can also merge the generated assembly with the Prolog runtime library to implement a completely independent static executable. The type of assembly target can be specified via the /target switch in the prologc executable.

In order to compile a Prolog program into a DLL assembly the /target:dll switch must be specified with the compiler. Every Prolog source code file is compiled into one .NET class. The class name has to be specified via the class/1 directive in the source file. Once compiled, every Prolog predicate is compiled into a public class member method. Since predicates can either be true or false, we set the return types of the compiled methods to be System.Boolean. The examples below show how a compiled Prolog program would look like if reverse-engineered in C#:

## Code Example: Fibonacci & Factorial in first-order logic 

<script src="https://gist.github.com/ahodroj/1177599.js"></script>

   
The class above would be compiled via the following command:

    prologc.exe /target:dll math.pro
    
Compiled .NET assembly: 
    public class MyMath { 
        public bool factorial(object arg1, object arg2) { /* ... */ } 
        public bool fib(object arg1, object arg2) { /* ... */ } 
    }




