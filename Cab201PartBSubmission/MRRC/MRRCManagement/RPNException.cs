using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Reverse Polish Notation exception class for RPN
    class RPNException : Exception
    {
        // Constructor which accepts in input message from the base exception class
        public RPNException(string message) : base(message)
        {

        }
    }
}
