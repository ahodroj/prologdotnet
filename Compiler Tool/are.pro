%------------------------------------------------------------------------------
% <copyright file="PrologCodeConstantAtom.cs" company="Axiom">
%     
%      Copyright (c) 2006 Ali Hodroj.  All rights reserved.
%     
%      The use and distribution terms for this source code are contained in the file
%      named license.txt, which can be found in the root of this distribution.
%      By using this software in any fashion, you are agreeing to be bound by the
%      terms of this license.
%     
%      You must not remove this notice, or any other, from this software.
%     
% </copyright>                                                                
%------------------------------------------------------------------------------



main :- repeat, 
		write('[Axiom]?-  '), 
		readp(Goal,Variables), 
		try(Goal,Variables).

try(G,novars) :- G, !, writeln(yes).
try(G,Vars) :- G, printVariables(Vars), more_variables, !.
try(_,_) :- writeln(no).

% Ask use for more variables
more_variables :- getnb(X), more(X).

% determine whether to backtrack or continue
more(10) :- !, writeln(yes).
more(59) :- !, nl, fail.
more(_) :- nl, writeln(yes).

% get key stroke from user
getnb(X) :- repeat, get0(X), X \= 32, !.

% Repeat
repeat.
repeat.

consult(X) :- consult(X).
