using System;

namespace ClydeConservationSystem
{
    public class FlyingBird : Bird
    {
        // Flying-Bird Specific Attributes
        public double WingSpan { get; private set; }
        public double FlightSpeed { get; private set; }

        // Constructor
        public FlyingBird(int birdId, string name, string type, string dateOfBirth, int dangerRating,
                          double insuranceValue, string diet, string habitat, string sex,
                          string feedingRequirement, string nestEnvironment, double wingSpan, double flightSpeed, int cageNumber)
            : base(birdId, name, type, dateOfBirth, dangerRating, insuranceValue, diet, habitat, sex, feedingRequirement, nestEnvironment, cageNumber)
        {
            WingSpan = wingSpan;
            FlightSpeed = flightSpeed;
        }

        // Implement Abstract Method
        public override void PrintDetails()
        {
            Console.WriteLine($"Flying Bird Details - ID: {BirdId}, Name: {Name}, Type: {Type}");
            Console.WriteLine($"Date of Birth: {DateOfBirth}, Danger Rating: {DangerRating}, Insurance Value: {InsuranceValue:C}");
            Console.WriteLine($"Diet: {Diet}, Habitat: {Habitat}, Sex: {Sex}, Cage Number: {CageNumber}");
            Console.WriteLine($"Feeding Requirement: {FeedingRequirement}, Nest Environment: {NestEnvironment}");
            Console.WriteLine($"Wing Span: {WingSpan}m, Flight Speed: {FlightSpeed} km/h");
        }
    }


}

