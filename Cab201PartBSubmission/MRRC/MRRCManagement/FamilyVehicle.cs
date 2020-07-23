namespace MRRCManagement
{
    // Derived class under the Vehicle class - Family vehicle. One attribute is defaulted
    public class FamilyVehicle : Vehicle
    {
        // This is the constructor of the family vehicle class which is invoked when an family vehicle object is created.
        // Upon creation, the dailyrate is defaulted to 80 dollars
        public FamilyVehicle(string registration, VehicleGrade grade, string make, string model, int year)
                                : base(registration, grade, make, model, year)
        {
            // DailyRate is set to 80 in this constructor.
            dailyRate = 80;
        }
    }
}
