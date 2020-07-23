using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    /* This class has been referenced and sourced from the code example in week 9 on Qut Cab201 blackboard 2020.
       Code example file name is: RPN.cs
       Link: https://blackboard.qut.edu.au/bbcswebdav/pid-8400584-dt-content-rid-31313831_1/xid-31313831_1 */

    #region RPN Constructor Operations
    // RPN class to convert query to infix and postfix tokens, which prepares for evaluation.
    public class RPN
    {
        // Arraylist to store operators and operands
        ArrayList operators;
        ArrayList operands;
        // Arraylist to store infix tokens
        public ArrayList InfixTokens { get; } = new ArrayList();
        // Arraylist to store Postfix tokens
        public ArrayList PostfixTokens { get; } = new ArrayList();

        public RPN(ArrayList query, Fleet fleet)
        {
            // Define valid tokens
            operators = new ArrayList();
            operands = new ArrayList();
            // Here we hardcode operators. Although operands takes from vehicleattribute list
            string[] op1 = { "AND", "OR", "(", ")" }; // operators and parentheses
            operators.AddRange(op1);
            string[] op2 = fleet.VehicleAttributes.ToArray(); // operands
            operands.AddRange(op2);
            // Create and instantiate a new empty Stack.
            Stack rpnStack = new Stack();

            // Apply dijkstra algorithm using a stack to convert infix to postfix notation (=rpn)
            InfixTokens.AddRange(query);
            foreach (string token in InfixTokens)
            {
                if (operands.Contains(token))
                {   // Move operands across to output
                    PostfixTokens.Add(token);
                }
                else if (token.Equals("("))
                {   // Push open parenthesis onto stack
                    rpnStack.Push(token);
                }
                else if (token.Equals(")"))
                {   // Pop all operators off the stack until the mathcing open parenthesis is found
                    while ((rpnStack.Count > 0) && !((string)rpnStack.Peek()).Equals("("))
                    {
                        PostfixTokens.Add(rpnStack.Pop());  // transfer operator to output
                        if (rpnStack.Count == 0)
                            throw new RPNException("Unbalanced parenthesis");
                    }
                    if (rpnStack.Count == 0)
                        throw new RPNException("Unbalanced parenthesis");
                    rpnStack.Pop(); // discard open parenthesis
                }
                else if (operators.Contains(token))
                {   // Push operand to the rpn stack after moving to output all higher or equal priority operators
                    while (rpnStack.Count > 0 && ((string)rpnStack.Peek()).Equals("AND"))
                    {
                        PostfixTokens.Add(rpnStack.Pop());  // Pop and add to output
                    }
                    rpnStack.Push(token); // Now put the operator onto the stack
                }
                else
                    throw new RPNException("Unrecognised token " + token);
            }
            // Copy what's left on the rpnStack
            while (rpnStack.Count > 0)
            {   // Msove to the output all remaining operators
                if (((string)rpnStack.Peek()).Equals("("))
                    throw new RPNException("Unbalanced parenthesis");
                PostfixTokens.Add(rpnStack.Pop());
            }
        }
        #endregion

    }
}
