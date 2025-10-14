using System;

namespace ClydeConservationSystem
{


    // Non-Flying Bird-Specific Attributes
    public class NonFlyingBird : Bird
    {
        // Non-Flying Bird Specific Attributes
        public double LandSpeed { get; private set; }
        public string PreferredHabitat { get; private set; }

        // Constructor
        public NonFlyingBird(int birdId, string name, string type, string dateOfBirth, int dangerRating,
                             double insuranceValue, string diet, string habitat, string sex,
                             string feedingRequirement, string nestEnvironment, double landSpeed, string preferredHabitat, int cageNumber)
            : base(birdId, name, type, dateOfBirth, dangerRating, insuranceValue, diet, habitat, sex, feedingRequirement, nestEnvironment, cageNumber)
        {
            LandSpeed = landSpeed;
            PreferredHabitat = preferredHabitat;
        }

        // Implement Abstract Method
        public override void PrintDetails()
        {
            Console.WriteLine($"Non-Flying Bird Details - ID: {BirdId}, Name: {Name}, Type: {Type}");
            Console.WriteLine($"Date of Birth: {DateOfBirth}, Danger Rating: {DangerRating}, Insurance Value: {InsuranceValue:C}");
            Console.WriteLine($"Diet: {Diet}, Habitat: {Habitat}, Sex: {Sex}");
            Console.WriteLine($"Feeding Requirement: {FeedingRequirement}, Nest Environment: {NestEnvironment}");
            Console.WriteLine($"Land Speed: {LandSpeed} km/h, Preferred Habitat: {PreferredHabitat}");
        }
    }


}



