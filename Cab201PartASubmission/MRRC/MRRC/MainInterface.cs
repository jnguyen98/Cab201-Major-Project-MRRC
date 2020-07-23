using MRRCManagement;
using System;
using System.Data;

namespace MRRC
{
    // This class is the main menu interface which holds the entire MRRC interface contained with sub-nested interfaces
    public class MainInterface
    {
        #region Fields
        // Crm and Fleet instance variables
        protected CRM Crm;
        protected Fleet Fleet;
        // Input key variable used to take user key input
        protected ConsoleKeyInfo inputkey;
        /// Key options
        protected const ConsoleKey Escape_Pressed = ConsoleKey.Escape;
        protected const ConsoleKey Backspace_Pressed = ConsoleKey.Backspace;
        protected const ConsoleKey H_Pressed = ConsoleKey.H;
        protected const ConsoleKey A_Pressed = ConsoleKey.A;
        protected const ConsoleKey B_Pressed = ConsoleKey.B;
        protected const ConsoleKey C_Pressed = ConsoleKey.C;
        protected const ConsoleKey D_Pressed = ConsoleKey.D;
        protected const ConsoleKey E_Pressed = ConsoleKey.E;
        protected const ConsoleKey Q_Pressed = ConsoleKey.Q;
        #endregion

        #region Constructor
        public MainInterface(Fleet Fleet, CRM Crm)
        {
            this.Fleet = Fleet;
            this.Crm = Crm;
        }
        #endregion

        #region Main MRRC Interface
        // Main MRRC interface with the full program and main menu, submenus and nested submenus
        public void MRRCInterface(bool clear = true)
        {
            if (clear)
                Console.Clear();

            // Display main menu options
            DisplayMainMenuOptions();
            // Read keyboard char input from user
            inputkey = Console.ReadKey();

            // Create and Instatiate Sub object instance from the Subinterface class
            // To have access to the submenus
            SubInterface sub = new SubInterface(Fleet, Crm);
            // Perform switch on key pressed to launch a specific submenu
            switch (inputkey.Key)
            {
                case A_Pressed:// prompt customer submenu
                    sub.SubMenu("Customer");
                    break;
                case B_Pressed: // fleet submenu
                    sub.SubMenu("Fleet");
                    break;
                case C_Pressed: // rental submenu
                    sub.SubMenu("Rentals");
                    break;
                case Escape_Pressed: // prompt quit message if escape is pressed
                    QuitMessage();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("You Pressed '{0}', Please enter a valid option\n", inputkey.Key.ToString());
                    // re-prompt the main menu
                    MRRCInterface(false);
                    break;
            }

        }
        #endregion

        #region Program Display messages upon start and quit
        // Quit message when user has exited the program
        protected void QuitMessage()
        {
            Console.Clear();
            Console.WriteLine("Program has quit. Thankyou for your time!");
            Console.WriteLine("Press any key to close program...");
            Console.ReadKey();
            // Terminate program
            Environment.Exit(0);

        }

        // Display Main menu options
        protected void DisplayMainMenuOptions()
        {
            Console.WriteLine("### Mates-Rates Rent-a-Car Operation Menu ###");
            Console.WriteLine("\nYou may press the ESC key at any menu to exit. " +
                              "Press the BACKSPACE key to return");
            Console.WriteLine("to the previous menu. Press the Q key to return to the parent menu.");
            Console.WriteLine("Press the H key to return home to the main menu.\n");
            string menu;
            menu = "a) Customer Management" + "\nb) Fleet Management"
                    + "\nc) Rental Management";
            Console.WriteLine("Please enter a character from the options below:\n");
            Console.Write(menu);
        } // end DisplayMenu
        #endregion

    }
}
