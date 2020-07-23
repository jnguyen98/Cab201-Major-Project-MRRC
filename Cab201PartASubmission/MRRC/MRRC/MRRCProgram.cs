using MRRCManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MRRC
{
    // Main Program class
    class MRRCProgram
    {
        public static void Main(string[] args)
        {
            //Create and instantiate new instance of Main Interface
            try
            {
                if (args.Length < 3)
                {
                    throw new Exception("Error: Insufficient command Line arguments");
                }
                Fleet Fleet = new Fleet(args[0], args[1]);
                CRM Crm = new CRM(args[2]);
                MainInterface Main = new MainInterface(Fleet, Crm);
                //Run MRRC main program interface
                Main.MRRCInterface();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }




        }
    }
}
