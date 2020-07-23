using MRRCManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MRRC
{
    // Main Program class
    public class MRRCProgram
    {
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 3) // If 3 files are not passed through the command line, prompt error.
                {
                    throw new FileException("Error: Insufficient command Line arguments");
                }
                // Create new object instance of CRM and fleet
                CRM Crm = new CRM(args[0]);
                Fleet Fleet = new Fleet(args[1], args[2]);
                // Create new object instance of Main and parse crm and fleet objects into the constructor class
                MainInterface Main = new MainInterface(Fleet, Crm);
                //Run MRRC main program interface
                Main.MRRCInterface(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }

        }
    }
}
