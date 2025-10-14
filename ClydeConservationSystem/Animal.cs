using System;

namespace ClydeConservationSystem
{
    // Abstract Class
    public abstract class Animal
    {
        public int AnimalId { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string DateOfBirth { get; private set; }
        public string DateOfAcquisition { get; private set; }
        public int DangerRating { get; private set; }
        public int CageNumber { get; set; }
        public double InsuranceValue { get; private set; }
        public string Diet { get; private set; }
        public string Habitat { get; private set; }
        public string Sex { get; private set; }

        // Primary Constructor
        protected Animal(int animalId, string name, string type, string dateOfBirth, string dateOfAqquisition, int dangerRating, int cageNumber,
                         double insuranceValue, string diet, string habitat, string sex)
        { // Constructor
            AnimalId = animalId;
            Name = name;
            Type = type;
            DateOfBirth = dateOfBirth;
            DateOfAcquisition = dateOfAqquisition;
            DangerRating = dangerRating;
            CageNumber = cageNumber;
            InsuranceValue = insuranceValue;
            Diet = diet;
            Habitat = habitat;
            Sex = sex;
        }

        //Overloaded Constructor
        protected Animal(int animalId, string name, string type)
        {
            AnimalId = animalId;
            Name = name;
            Type = type;
            DateOfBirth = "Unknown";
            DateOfAcquisition = "Unknown";
            DangerRating = 1; // Default value
            CageNumber = 0; // Not assigned to a cage
            InsuranceValue = 0.0; // Default value
            Diet = "Unknown";
            Habitat = "Unknown";
            Sex = "Unknown";
        }

        // Abstract Method
        public abstract void PrintDetails();
    }
}




