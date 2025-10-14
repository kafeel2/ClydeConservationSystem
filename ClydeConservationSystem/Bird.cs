using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    public abstract class Bird
    {
        // Bird-Specific Attributes
        public int BirdId { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string DateOfBirth { get; private set; }
        public int DangerRating { get; private set; }
        public double InsuranceValue { get; private set; }
        public string Diet { get; private set; }
        public string Habitat { get; private set; }
        public string Sex { get; private set; }
        public string FeedingRequirement { get; private set; }
        public string NestEnvironment { get; private set; }
        public int CageNumber { get; set; }

        // Constructor
        protected Bird(int birdId, string name, string type, string dateOfBirth, int dangerRating,
                       double insuranceValue, string diet, string habitat, string sex,
                       string feedingRequirement, string nestEnvironment, int cageNumber) // Parameters for the Bird class

        // Assigning the parameters to the attributes of the Bird class
        {
            BirdId = birdId;
            Name = name;
            Type = type;
            DateOfBirth = dateOfBirth;
            DangerRating = dangerRating;
            InsuranceValue = insuranceValue;
            Diet = diet;
            Habitat = habitat;
            Sex = sex;
            FeedingRequirement = feedingRequirement;
            NestEnvironment = nestEnvironment;
            CageNumber = cageNumber;
        }

        // Method to print the details of the bird
        public abstract void PrintDetails();
    }

}
