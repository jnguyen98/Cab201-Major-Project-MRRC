using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRRCManagement
{
    // File exception class to deal with File input or insufficient Commandline Arguments
    public class FileException : Exception
    {
        // Constructor which accepts in input message from the base exception class
        public FileException(string message) : base(message)
        {

        }
    }
}
