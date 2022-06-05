# Overview

Prolog is a logic programming language associated with artificial intelligence and computational linguistics. Prolog has its roots in first-order logic, a formal logic, and unlike many other programming languages, Prolog is intended primarily as a declarative programming language: the program logic is expressed in terms of relations, represented as facts and rules. A computation is initiated by running a query over these relations.

Prolog.NET provides an implementation of Prolog programming language on the .NET Framework. It includes an enhanced compiler with language extensions and a code generator targeting Microsoftâ€™s Intermediate Language (MSIL).

# Language Design / Compiler Architecture
The Prolog.NET system consists of a Prolog compiler and a runtime environment. The compiler (prologc.exe) is responsible for compiling Prolog programs into either abstract machine code to be executed by the runtime environment or .NET intermediate language executed directly by the .NET Common Language Runtime. The runtime environment is a .NET implementation of a Warren Abstract Machine with an extended instruction set that facilitates interoperability between Prolog and .NET. 

# Code Examples

## Fibonacci Series & Factorial implementation in first-order logic (Prolog)
```
:- class('MyMath').

factorial(0,1).
factorial(N,F) :- N > 0, M is N - 1, factorial(M,Fm), F is N * Fm.

fib(0,1).
fib(1,1).
fib(N,F) :- N > 1, N1 is N - 1, N2 is N - 2, fib(N1,F1), fib(N2,F2), F is F1+F2
```
To compile the class above into a library/assembly:
```  prologc.exe /target:dll math.pro```

which internally looks like: 

```
public class MyMath { 
      public bool factorial(object arg1, object arg2) { /* ... */ } 
      public bool fib(object arg1, object arg2) { /* ... */ } 
}
````
# Data Types and Interoperability
Prolog.NET Runtime provides the AbstractTerm class type which is the equivalent of a Prolog variable. AbstractTerm acts as a container for a value obtained after unifying it with a predicate term. In order to use AbstractTerm, the Axiom.Runtime namespace must be used. The following is a list of Prolog and .NET equivalent data types that can be used to call a Prolog predicate:

| Prolog Data Type | .NET Data Type |
| --- | ----------- |
| Integer |	System.Int32 |
| Constant |	System.String |
| String	| System.String |
| List	| System.Collections.ArrayList |
| Structure |	StructureTerm |
| Variable	| AbstractTerm |

An example of data conversion is show below: 

```
using System;
using Prolog.Assembly;
using Axiom.Runtime; // required only for AbstractTerm

namespace PrologAndMath
{
    class Program
    {
        static void Main(string[] args)
        {
            MyMath math = new MyMath();
            // Calculate the factorial 
            AbstractTerm a = new AbstractTerm();
            math.factorial(3, a);
            Console.WriteLine("Factorial of 3 is: " + a);
            // calculate the fifth fibonacci (should be 8)
            AbstractTerm fibValue = new AbstractTerm();
            math.fib(5, fibValue); // this is like fib(5, X) in Prolog
            Console.WriteLine("Fibonacci of 3 is: " + fibValue);
        }
    }
}
```

