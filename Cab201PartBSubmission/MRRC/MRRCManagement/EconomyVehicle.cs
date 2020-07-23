namespace MRRCManagement
{
    // Derived class under the Vehicle class - Economy vehicle. One Attribute is defaulted
    public class EconomyVehicle : Vehicle
    {
        // This is the constructor of the economy vehicle class which is invoked when an economy vehicle object is created.
        // Upon creation, the transmissiontype is defaulted to automatic
        public EconomyVehicle(string registration, VehicleGrade grade, string make, string model, int year)
                                : base(registration, grade, make, model, year)
        { 
            // Set the default transmission of this class to automatic
            transmission = TransmissionType.Automatic;
        }
    }
}
