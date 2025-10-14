using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    public class Reptile : Animal
    {
        // Reptile-Specific Attributes
        public double TankTemperature { get; private set; }
        public string Environment { get; private set; }

        // Constructor
        public Reptile(int animalId, string name, string type, string dateOfBirth, string dateOfAcquisition, int dangerRating, int cageNumber,
                       double insuranceValue, string diet, string habitat, string sex, double tankTemperature, string environment)
            : base(animalId, name, type, dateOfBirth, dateOfAcquisition, dangerRating, cageNumber, insuranceValue, diet, habitat, sex)
        {
            TankTemperature = tankTemperature;
            Environment = environment;
        }

        // Implement Abstract Method
        public override void PrintDetails()
        {
            Console.WriteLine($"Reptile Details - ID: {AnimalId}, Name: {Name}, Type: {Type}");
            Console.WriteLine($"Date of Birth: {DateOfBirth}, Danger Rating: {DangerRating}, Insurance Value: {InsuranceValue:C}");
            Console.WriteLine($"Diet: {Diet}, Habitat: {Habitat}, Sex: {Sex}, Cage Number: {CageNumber}");
            Console.WriteLine($"Tank Temperature: {TankTemperature}°C, Environment: {Environment}");
        }
    }

}
