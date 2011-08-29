using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using Axiom.Runtime;

namespace Axiom.Runtime.Builtins
{
    public class TermEvaluator
    {
        public static double Evaluate(AbstractTerm t)
        {
            double result = 0;

            AbstractTerm term = t.Dereference();

            if (term.IsConstant)
            {
                int r;
                double d;
                if (Int32.TryParse((string)term.Data(), out r))
                {
                    return r;
                }
                else if (Double.TryParse((string)term.Data(), out d))
                {
                    return d; 
                }
                else
                {
                    switch ((string)term.Data())
                    {
                        case "pi":
                            result = Math.PI;
                            break;
                        case "e":
                            result = Math.E;
                            break;
                        case "cputime":
                            Process p = Process.GetCurrentProcess();
                            result = (p.TotalProcessorTime.TotalMilliseconds/1000);
                            break;
                        default:
                            throw new Exception("TermEvaluator: cannot evaluate string " + term.Data());
                    }
                }
            }
            else if (term.IsReference)
            {
                throw new Exception("TermEvaluator: cannot evaluate a reference term.");
            }
            else if (term.IsStructure)
            {
                AbstractTerm op1 = (AbstractTerm)term.Next;
                AbstractTerm op2 = (AbstractTerm)term.Next.Next;

                switch (term.Name)
                {
                    case "+":
                        result = Evaluate(op1) + Evaluate(op2);
                        break;
                    case "-":
                        result = Evaluate(op1) - Evaluate(op2);
                        break;
                    case "*":
                        result = Evaluate(op1) * Evaluate(op2);
                        break;
                    case "/":
                        if (Evaluate(op2) == 0)
                        {
                            throw new Exception("TermEvaluator: Division by zero error.");
                        }
                        result = Evaluate(op1) / Evaluate(op2);
                        break;
                    case "^":
                        result = Math.Pow(Evaluate(op1), Evaluate(op2));
                        break;
                    case "cos":
                        result = Math.Cos(Evaluate(op1));
                        break;
                    case "sin":
                        result = Math.Sin(Evaluate(op1));
                        break;
                    case "tan":
                        result = Math.Tan(Evaluate(op1));
                        break;
                    case "log":
                        result = Math.Log(Evaluate(op1));
                        break;
                }
            }
            return result;
        }
    }
}
