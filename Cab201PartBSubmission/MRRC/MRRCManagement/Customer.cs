using System;

namespace MRRCManagement
{

    // This Customer class contains an enumeration type, Constructor, properties and methods  
    // that will be useful as a template for performing operations on the customers csv file.
    public class Customer
    {
        #region Gener Enum
        // Defined Gender public enum
        public enum Gender
        {
            Male,
            Female,
            Other,
        }
        #endregion

        #region Properties
        // Get; set; properties which are useful for modifying a customer
        public int ID { get; set; }
        public string title { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public Gender gender { get; set; }
        public DateTime DOB { get; set; }
        #endregion

        #region Constructor
        // Main customer constructor taking in mandatory parameters
        public Customer(int ID, string title, string firstName, string lastName, Gender gender, DateTime DOB)
        {
            this.ID = ID;
            this.title = title;
            this.firstName = firstName;
            this.lastName = lastName;
            this.gender = gender;
            this.DOB = DOB;
        }
        #endregion

        #region Methods
        // This method should return a CSV representation of the customer that is consistent
        // with the provided data files.
        public string ToCSVstring()
        {
            return ID + "," + title + "," + firstName + "," + lastName + "," + gender + "," + DOB.Date.ToString("d");
        }

        // This method should return a string representation of the attributes of the customer.
        public override string ToString()
        {
            return "<" + ID + ">," + "<" + title + ">," + "<" + firstName + ">," + "<" + lastName + ">," + "<" + gender + ">," + "<" + DOB.Date.ToString("d") + ">";
        }
        #endregion
    }
}
