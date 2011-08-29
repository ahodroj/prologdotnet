

using System;
using System.Collections;
using NUnit.Framework;
using Axiom.Runtime;
using Axiom.Runtime.Instructions;

namespace Axiom.Runtime.UnitTests
{
    [TestFixture]
    public class _AMInstructionSet
    {
        string[] instructions = {
                "put_variable",
                "put_value",
                "put_unsafe_value",
                "put_structure",
                "put_list",
                "put_constant",

                "get_variable",
                "get_value",
                "get_structure",
                "get_list",
                "get_constant",

                "set_variable",
                "set_value",
                "set_local_value",
                "set_constant",
                "set_void",

                "unify_variable",
                "unify_value",
                "unify_local_value",
                "unify_constant",
                "unify_void",

                "allocate",
                "deallocate",
                "call",
                "execute",
                "proceed",

                "try_me_else",
                "retry_me_else",
                "trust_me",

                "cut",
                "fcall",
                "bcall",
            };
        [Test]
        public void IsValidInstruction()
        {
            AMInstructionSet am = new AMInstructionSet();

            foreach (string s in instructions)
            {
                Assert.IsTrue(am.IsValidInstruction(s));
                Assert.IsTrue(am.isValidInstruction(s));
            }
            
        }

        [Test]
        public void CreateInstruction()
        {
            AMInstructionSet am = new AMInstructionSet();

            foreach (string s in instructions)
            {
                AbstractInstruction i = null;

                if (s == "set_void" || s == "unify_void")
                {
                    i = am.CreateInstruction(s, "1");
                    Assert.AreEqual(s, i.Name());

                }
                else
                {
                    i = am.CreateInstruction(s, "f/1", "1", "X2", "X3");
                    Assert.AreEqual(s, i.Name());
                }
                

            }
        }

    }
}