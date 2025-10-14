using ClydeConservationSystem;
using System.Xml.Linq;

public class Mammal : Animal
{
    // Mammal-Specific Attributes
    public string MateName { get; private set; }
    public string? BirthDate { get; private set; } // Nullable for males or no births

    // Constructor
    public Mammal(int animalId, string name, string type, string dateOfBirth, string dateOfAcquisition, int dangerRating, int cageNumber,
                  double insuranceValue, string diet, string habitat, string sex, string mateName, string? lastBirthDate)
        : base(animalId, name, type, dateOfBirth, dateOfAcquisition, dangerRating, cageNumber, insuranceValue, diet, habitat, sex)
    {
        MateName = mateName;
        BirthDate = lastBirthDate; // Null if not applicable
    }

    // Implement Abstract Method
    public override void PrintDetails()
    {
        Console.WriteLine($"Mammal Details - ID: {AnimalId}, Name: {Name}, Type: {Type}");
        Console.WriteLine($"Date of Birth: {DateOfBirth}, Danger Rating: {DangerRating}, Insurance Value: {InsuranceValue:C}");
        Console.WriteLine($"Diet: {Diet}, Habitat: {Habitat}, Sex: {Sex}, Cage Number: {CageNumber}");
        Console.WriteLine($"Mate Name: {MateName}, Last Birth Date: {BirthDate ?? "N/A"}");
    }

}

