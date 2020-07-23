using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // Search exception class for the Search functionality method in fleet class
    class SearchException : Exception
    {
        // Constructor which accepts in input message from the base exception class
        public SearchException(string message) : base(message)
        {

        }
    }
}
