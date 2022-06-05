# Table of Contents
* [What is Prolog?](https://github.com/ahodroj/prologdotnet#overview)
* [Prolog & Logic Programming Use Cases](https://github.com/ahodroj/prologdotnet/blob/master/README.md#prolog-and-logic-programming-use-cases)
* [Quickstart](https://github.com/ahodroj/prologdotnet/blob/master/README.md#quickstart)
* 

# What is Prolog?

Prolog is a logic programming language associated with artificial intelligence and computational linguistics. Prolog has its roots in first-order logic, a formal logic, and unlike many other programming languages, Prolog is intended primarily as a declarative programming language: the program logic is expressed in terms of relations, represented as facts and rules. A computation is initiated by running a query over these relations.

Prolog.NET provides an implementation of Prolog programming language on the .NET Framework. It includes an enhanced compiler with language extensions and a code generator targeting the .NET runtime. A brief [introduction to Prolog can be found here](http://www.cs.toronto.edu/~hojjat/384w10/PrologTutorial1.pdf)


# Prolog and Logic Programming Use Cases
Prolog is well-suited for specific tasks that benefit from rule-based logical queries. Prolog is used a lot in NLP, particularly in syntax and computational semantics.

* [Theorem proving](https://courses.cs.washington.edu/courses/cse341/98wi/CurrentQtr/lectures/pro1.pdf)
* [Natural language processing](https://cs.union.edu/~striegnk/courses/nlp-with-prolog/html/)
* [Pattern matching](https://cs.union.edu/~striegnk/courses/nlp-with-prolog/html/)
* [Combinatorial test case generation](http://www.sfu.ca/~tjd/383summer2019/prolog_combinatorial.html)
* [Symbolic Computation / Calculus](https://link.springer.com/chapter/10.1007/978-3-642-83213-0_6)
* [Deductive Reasoning](https://link.springer.com/chapter/10.1007/978-3-642-83213-0_6)



# Quickstart

## Prolog code to .NET runtime
```
% implementaion of fibonacci series and factorial functions in prolog
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
## Data Types and Interoperability
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
## Generating Executables
The Prolog.NET compiler can compile Prolog programs into .NET executable assemblies via the /target:exe switch. By default, the executable generated will require having Prolog.NET installed in order to execute; this is because it makes use of the Runtime library (which is installed in the GAC). However, you can provide the /static switch to the compiler to build a static executable that can be executed on a system without the Prolog.NET runtime installed. In Prolog.NET, the main/0 predicate is the entry point of the executable at runtime. For instance, the famous hello world program:

```
% hello world
main :- write(‘Hello, World!’), nl.
```
compile into an exe through: ```prologc.exe /target:exe hello.pro ```

## Writing .NET code through first-order logic predicates (calling .NET from Prolog)

There are five built-in predicates in Prolog.NET that allow using .NET objects and methods from Prolog: object/2, invoke/3, get_property/3, set_property/3, and ::/2. This section describes how each can be used in a Prolog program. In order to use the object-oriented predicates against classes and objects in an assembly or namespace, the using/1 or assembly/1 directives should be specified. The using/1 predicate takes a .NET namespace as its only argument, while the assembly/1 takes an assembly name. They both load an assembly so that the class types defined in it can be used from Prolog. The following are examples of each:

```
using('System.Collections').   % reference a namespcae

assembly('MyMath.dll').   % reference an assembly
```

### Object instantiation
When the Prolog.NET runtime environment searches for the class type to be instantiated, it first looks in the assemblies that have been loaded via using/1. If not found, it will then look in the assemblies loaded via assembly/1. To instantiate an object from .NET the object/1 built-in predicate is used:

```
% Create an object of type ClassName, bind it to the ObjectTerm variable
object(+ClassName, -ObjectTerm).
```

The first argument is an atom of the class type which is created and bound to the second argument, known as an object term. The following is an example of creating an ArrayList in Prolog:

```
arraylist(X) :- object('System.Collections.ArrayList', X). 
```

### Calling a Class/Object Method
There are two ways to call methods from Prolog, either using the invoke/3 or ::/2 built-in predicate. The advantage of the latter is that it provides a more OOP-familiar syntax and its method’s return value can be evaluated using the is/2 predicate.

The invoke/3 predicate takes three arguments. The first argument is the object term (or the class type name in case we are invoking a static method). The second argument is a functor representing the method name as well as aguments passed to it. Finally, the third term is unified with the value returned from invoked:

```
invoke(+ClassTypeOrObject, +MethodName(+Arguments...),?ReturnValue).
```

The following is an example of invoking the Add() method from a hypothetical ```Calc``` class: 

```
% create an object of type Calc, then call the Calc.Add() method

dotnet_add(X,Y,Z) :- object('Calc', Obj), invoke(Obj, 'Add'(X,Y), Z)
```

Since the return value is unified with the last argument, querying the following two goals is similar:

```
% first definition, another way of saying:
%  The sum of 1 and 2 shall be 3
add :- dotnet_add(1,2,Z), Z = 3. 

% Another similar definition  
add2 :- dotnet_add(1,2,3).
```

Another way to invoke a method is by using the ::/2 built-in predicate, which has the following syntax:

```
+ClassTypeOrObject :: +MethodName(+Args...)  
```

For instance, Calling the Console.WriteLine() method the following way:

```
% Console.WriteLine("Hello, World!"); 
'Console'::'WriteLine'('Hello, World!'). 
```

The ::/2 predicate can be used in an is/2 expression. For example, rew-writing the above dotnet_add/3 rule using ::/2 would look like:

```
% create object O of type 'Calc', then bind the Z variable to the return value of Add()
dotnet_add(X,Y,Z) :- object('Calc',O), Z is O::'Add'(X,Y).
```

### Getting/Setting Properties
You can get and modify that values of class properties, which are responsible for setting and getting non-public variables in a class. The two predicates available to achieve this are: get_property/3 and set_property/3. Both of these predicates are used almost the same way as the invoke/3 predicate. The predicate signatures are:

```
% get the property value of class or object and unify it with a variable
get_property(+ClassTypeOrObject, +PropertyName, ?Value) 

% set the property of class or object to Value
set_property(+ClassTypeOrObject, +PropertyName, +Value)
```

The first argument term of each predicate is the instantiated object or a class type (in case we are getting/setting a static property of a class). The second argument is the property name. The third argument in get_property/3 is unified with the value returned from the property, while in set_property/3, it’s the value that the property needs to be set to. The following is an example of getting a property that checks if a System.Collections.ArrayList object has no elements by asserting that the ArrayList.Count property returns 0:

```
% TRUE if X.Count == 0 (X is an object of type ArrayList)
empty_array_list(X) :- get_property(X, 'Count', 0).
```
The following is an example of using set_property to set the Max property of a Calculator class:

```
% Similar to the C# code
%    Calculator.Max = 3;
set_property('Calculator', 'Max', 3).
```

# Built-In/Standard Prolog Predicates
The Prolog.NET library currently has a limited set of built-in predicates that are common to Prolog implementations. The following standard Prolog predicates are available:

## I/O
```
write/1 
writeln/1 
nl/0 
get0/1 
skip/1 
put/1 
```

## Comparison
```
=/=/2 
=:=/2 
>=/2 
=</2 
</2 
>/2
```

## Control
```
call/1
```

## Equality & Unification
```
=/2 
\=/2
```

## Meta-Logic
```
is/2 
atom/1 
bound/1 
char/1 
free/1 
var/1 
nonvar/1 
integer/1
```

## .NET Interoperability
```
object/2 
invoke/3 
::/2 
get_property/3 
set_property/3
```


