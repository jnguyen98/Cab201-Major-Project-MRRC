using MRRCManagement;
using System;
using System.Globalization;
using System.Linq;

namespace MRRC
{
    // This derived submenu interface class contains CRM options for addition, modification and deletion of customers
    public class InnerSubInterfaceCRM : SubInterface
    {
        #region Fields
        // String variables to read inputs from user in the crm menu
        private string id;
        private string title;
        private string firstName;
        private string lastName;
        private string gender;
        private string dob;
        // The 5 variables below will undergo appropriate validation testing. They will be parsed into
        // the out parameters of the tryparse methods
        private DateTime DOB;
        private Customer.Gender Gender;
        private int DeleteRecordID;
        private int IDtoMofify;
        #endregion

        #region Constructor
        public InnerSubInterfaceCRM(Fleet fleet, CRM crm) : base(fleet, crm) { }
        #endregion

        #region Validation user input for Customer
        // Method checks validation user input for the date of birth
        private void DOBValidationInput()
        {
            bool dobIsValid;
            do
            {
                Console.Write("DOB*: ");
                dob = Console.ReadLine();
                dobIsValid = DateTime.TryParseExact(dob, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DOB);
                if (!dobIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not in the correct format for the date of birth", dob);
                    Console.WriteLine("Please enter in the following format: dd/MM/YYYY\n");
                }
                else
                {
                    string dateA = DOB.Date.ToString("d");
                    string dateB = DateTime.Now.Date.ToString("d");
                    if (dateA.CompareTo(dateB) == 1)
                    {
                        Console.WriteLine("\nYou entered {0}, which not a valid Date.", dateA);
                        Console.WriteLine("Please enter a Date which is less than or equal to {0}\n", dateB);
                        dobIsValid = false;
                    }
                }
            } while (!dobIsValid);
        }

        // Method checks validation user input for the Gender
        private void GenderValidationInput()
        {
            bool genderIsValid;
            do
            {
                Console.Write("Gender*: ");
                gender = Console.ReadLine();
                genderIsValid = Enum.TryParse(gender, true, out Gender);

                if (!genderIsValid)
                {
                    Console.WriteLine("\nYou entered {0}, which is not a member of the gender Enum", gender);
                    Console.WriteLine("Please enter a defined gender in the following format: Male, Female or Other\n");
                }
            } while (!genderIsValid);
        }

        // Method checks validation user input when modifying the crm file
        private void ModifyCRMValidationInput()
        {
            bool IDisValid;
            do
            {
                Console.Write("Please enter the customer ID of the customer to be modified: ");
                id = Console.ReadLine();
                // Here we use ID to modify as a means for the operator to input customer ID needed to be modified.
                IDisValid = int.TryParse(id, out IDtoMofify);
                if (IDtoMofify < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive integer", IDtoMofify);
                    Console.WriteLine("Enter an ID which is a positive integer\n");
                    IDisValid = false;
                }
                else if (!int.TryParse(id, out IDtoMofify))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an integer", id);
                    Console.WriteLine("Enter an ID which is an integer\n");
                    IDisValid = false;
                }
                else if (Fleet.IsRenting(IDtoMofify))
                {
                    Console.WriteLine("\nCould not modify customer ID '{0}' as this customer is currently renting a vehicle. Try another ID.\n",
                                IDtoMofify);
                    IDisValid = false;
                }
            } while (!IDisValid);
        }
        // Method checks validation user input for the ID when modifying the customer ID to a different ID
        private void ModifyIDValidationInput()
        {
            bool IDisValid;
            do
            {
                Console.Write("ID*: ");
                id = Console.ReadLine();
                // Here we re-use the IDtoModify variable but as the new ID from the old ID modified into.
                IDisValid = int.TryParse(id, out IDtoMofify);
                if (IDtoMofify < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive integer", IDtoMofify);
                    Console.WriteLine("Enter an ID which is a positive integer\n");
                    IDisValid = false;
                }
                else if (!int.TryParse(id, out IDtoMofify))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an integer", IDtoMofify);
                    Console.WriteLine("Enter an ID which is an integer\n");
                    IDisValid = false;
                }
                else if (Crm.CustomerList.Any(archivedCustomer => archivedCustomer.ID == IDtoMofify))
                {
                    Console.WriteLine("\nID number '{0}' cannot be accepted as it exists in the customer list", IDtoMofify);
                    Console.WriteLine("Try entering a different Customer ID which is unique.\n");
                    IDisValid = false;
                }
            } while (!IDisValid);
        }

        // Method checks validation user input when deleting a record from crm
        private void DeleteCRMValidationInput()
        {
            bool IDisValid;
            do
            {
                Console.Write("Please enter the customer ID of the customer to be deleted: ");
                id = Console.ReadLine();
                IDisValid = int.TryParse(id, out DeleteRecordID);
                if (DeleteRecordID < 0)
                {
                    Console.WriteLine("\nYou entered '{0}', which is not a positive integer", DeleteRecordID);
                    Console.WriteLine("Enter an ID which is a positive integer\n");
                    IDisValid = false;
                }
                else if (!int.TryParse(id, out DeleteRecordID))
                {
                    Console.WriteLine("\nYou entered '{0}', which is not an integer", id);
                    Console.WriteLine("Enter an ID which is an integer\n");
                    IDisValid = false;
                }

            } while (!IDisValid);
        }
        #endregion

        #region Add new customer submenu interface
        // Method which holds the interface for adding a customer to the crm file
        public void NewCustomerSubmenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### New Customer Submenu ###\n");
            Console.WriteLine("Please fill the following fields (fields with * are required)\n");
            Console.Write("Title*: ");
            title = StringToTitleCase(Console.ReadLine());
            Console.Write("FirstName*: ");
            firstName = StringToTitleCase(Console.ReadLine());
            Console.Write("LastName*: ");
            lastName = StringToTitleCase(Console.ReadLine());
            GenderValidationInput();
            DOBValidationInput();

            // Determine the highest ID number in the list
            int HighestID;
            // if the list has any customer, we determine the max ID using the max method on the list of objects
            if (Crm.CustomerList.Count() > 0)
            {
                HighestID = Crm.CustomerList.Max(Customer => Customer.ID);
            }// If the list has no customers, we make the id to be -1, so when the first customer is added, it will 
            // increment by one, which will be 0 (-1 + 1 = 0).
            else
            {
                HighestID = -1;
            }

            // The customer id number must be unqiue, so we will make it equal to the highest ID number in the crm list and 
            // increment it by one
            int CustomerIDNumber = HighestID + 1;

            // Add new customer
            Customer newCustomer = new Customer(CustomerIDNumber, title, firstName, lastName, Gender, DOB);
            if (Crm.AddCustomer(newCustomer))
            {
                Console.Write("\nSuccessfully created new customer '{0} - {1} {2} {3}' and added to customer list",
                           CustomerIDNumber, title, firstName, lastName);
                Crm.SaveToFile();
            }
            // If user presses any key, prompt the end of the menu
            LastMRRCscreen(() => SubMenu("Customer"), () => NewCustomerSubmenu());
        }
        #endregion

        #region Modify customer submenu interface
        // Method to modify a specific customer which exists in the crm file
        public void ModifyCustomerSubmenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Modify Customer Submenu ###\n");
            ModifyCRMValidationInput();
            // Return the customer which matches the provided ID to modify and assign to modifiedcustomer object
            Customer customer = Crm.GetCustomer(IDtoMofify);
            // if the returned customer in the modified customer object is not null, proceed to modify
            if (customer != null)
            {
                // Store old customers ID, title, first and last names into variables
                int OldID = customer.ID;
                string OldTitle = customer.title, OldFirstName = customer.firstName, OldLastName = customer.lastName;
                // Set the customer ID to the highest id value + 1, in case the customer wants to retain his/her ID and only wants
                // to modify the fields. We set the ID to this so that the validation doesnt prompt an error that the ID wanting 
                // to be retained already exists. If the ID is retained, there is still unique ID's in the list
                int HighestID = Crm.CustomerList.Max(Customer => Customer.ID) + 1;
                customer.ID = HighestID;
                // Save the changed ID value in case customer decideds to retain the same ID in the modification menu.
                Crm.SaveToFile();
                ModifyIDValidationInput();
                customer.ID = IDtoMofify;
                Console.Write("Title*: ");
                title = StringToTitleCase(Console.ReadLine());
                customer.title = title;
                Console.Write("FirstName*: ");
                firstName = StringToTitleCase(Console.ReadLine());
                customer.firstName = firstName;
                Console.Write("LastName*: ");
                lastName = StringToTitleCase(Console.ReadLine());
                customer.lastName = lastName;
                GenderValidationInput();
                customer.gender = Gender;
                DOBValidationInput();
                customer.DOB = DOB;
                Console.Write("\nSuccessfully modified customer '{0} - {1} {2} {3}' to '{4} {5} {6} {7}' within the customers list",
                           OldID, OldTitle, OldFirstName, OldLastName, IDtoMofify, title, firstName, lastName);
                Crm.SaveToFile();
                LastMRRCscreen(() => SubMenu("Customer"), () => ModifyCustomerSubmenu());
            }
            // If the returned customer in the object is null, then the customer does not exist
            else
            {
                Console.Clear();
                Console.WriteLine("Customer with an ID of '{0}' does not exist in the Customers List\n", IDtoMofify);
                ModifyCustomerSubmenu(false);
                LastMRRCscreen(() => SubMenu("Customer"), () => ModifyCustomerSubmenu());
            }
        }
        #endregion

        #region Remove customer submenu interface
        // Method to remove customer from the crm file
        public void RemoveCustomerSubmenu(bool clear = true)
        {
            if (clear)
                Console.Clear();

            Console.WriteLine("### Delete Customer Submenu ###\n");
            DeleteCRMValidationInput();
            // Ensure that the desired ID to be deleted exists in the customer list to execute deletion
            if (Crm.CustomerList.Any(customer => customer.ID == DeleteRecordID))
            {
                // deletion is successful if customer is not renting a vehicle
                if (Crm.RemoveCustomer(DeleteRecordID, Fleet))
                {
                    Console.WriteLine($"\nSuccessfully deleted the Customer with an ID of '{DeleteRecordID}'");
                }
                else
                {
                    Console.WriteLine($"\nDeletion Unsuccessful. Customer '{DeleteRecordID}' is currently renting a vehicle.");
                }
                LastMRRCscreen(() => SubMenu("Customer"), () => RemoveCustomerSubmenu());
            }
            // If the desired ID to be deleted is not in the list, prompt a message
            else
            {
                Console.Clear();
                Console.WriteLine("Customer with an ID of '{0}' does not exist in the Customers List\n", DeleteRecordID);
                RemoveCustomerSubmenu(false);
                LastMRRCscreen(() => SubMenu("Customer"), () => RemoveCustomerSubmenu());
            }
        }
        #endregion
    }
}
