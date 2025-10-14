using System;
using System.Collections.Generic;
using System.Linq;

namespace ClydeConservationSystem
{
    public class HeadKeeper
    {
        public int KeeperId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Position { get; private set; }
        public List<int> AssignedCages { get; private set; }

        // ✅ Corrected Constructor: Uses assignedCages list properly
        public HeadKeeper(int keeperId, string firstName, string lastName, string position, List<int> assignedCages = null)
        {
            KeeperId = keeperId;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            AssignedCages = assignedCages ?? new List<int>(); // ✅ Prevents null reference errors
        }

        // ✅ Empty constructor initializes AssignedCages
        public HeadKeeper()
        {
            AssignedCages = new List<int>(); // ✅ Prevents null error
        }

        // ✅ Corrected Constructor: Initializes AssignedCages
        public HeadKeeper(int keeperId, string? firstName, string? lastName, string? position)
        {
            KeeperId = keeperId;
            FirstName = firstName;
            LastName = lastName;
            Position = position;
            AssignedCages = new List<int>(); // ✅ Prevents null error
        }

        public bool AssignCage(int cageNumber)
        {
            if (AssignedCages.Contains(cageNumber))
            {
                Console.WriteLine("Cage already assigned to this keeper.");
                return false;
            }
            AssignedCages.Add(cageNumber);
            return true;
        }

        public bool AssignKeeperToCage(List<Cage> cages)
        {
            if (cages.Count == 0)
            {
                Console.WriteLine("No cages available to assign.");
                return false;
            }

            Console.WriteLine("\nAvailable Cages:");
            foreach (var cage in cages)
            {
                Console.WriteLine($"Cage Number: {cage.CageNumber}, Description: {cage.Description}");
            }

            Console.Write("Enter the Cage Number to assign: ");
            if (!int.TryParse(Console.ReadLine(), out int cageNumber))
            {
                Console.WriteLine("Invalid Cage Number.");
                return false;
            }

            var selectedCage = cages.Find(c => c.CageNumber == cageNumber);
            if (selectedCage == null)
            {
                Console.WriteLine("Cage not found.");
                return false;
            }

            // ✅ Fix: If the keeper is already assigned to this cage, prevent duplicates
            if (AssignedCages.Contains(cageNumber))
            {
                Console.WriteLine($"Cage {cageNumber} is already assigned to this keeper.");
                return false; // Stops the method here
            }

            // Check if the cage already has another keeper assigned
            if (selectedCage.ResponsibleKeeper != null)
            {
                Console.WriteLine($"Cage {cageNumber} is already assigned to Keeper {selectedCage.ResponsibleKeeper.KeeperId}.");
                return false;
            }

            // Assign keeper to the cage
            if (!selectedCage.AssignKeeper(this))
            {
                Console.WriteLine("Failed to assign keeper to the cage.");
                return false;
            }

            // ✅ Now safely add the cage to the keeper's AssignedCages list
            AssignedCages.Add(cageNumber);
            Console.WriteLine($"Keeper {FirstName} assigned to Cage {cageNumber} successfully.");

            return true;
        }


        public void AddKeeper(List<HeadKeeper> keepers)
        {
            Console.Write("Enter Keeper ID: ");
            if (!int.TryParse(Console.ReadLine(), out int keeperId))
            {
                Console.WriteLine("Invalid Keeper ID. Please enter a numeric value.");
                return;
            }

            if (keepers.Any(k => k.KeeperId == keeperId))
            {
                Console.WriteLine($"A keeper with ID {keeperId} already exists.");
                return;
            }

            Console.Write("Enter First Name: ");
            string firstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            string lastName = Console.ReadLine();

            Console.Write("Enter Position: ");
            string position = Console.ReadLine();

            // ✅ Ensures AssignedCages is initialized
            var newKeeper = new HeadKeeper(keeperId, firstName, lastName, position, new List<int>());
            keepers.Add(newKeeper);

            Console.WriteLine($"Keeper {firstName} {lastName} added successfully.");
        }

        public static void RemoveKeeper(List<HeadKeeper> keepers, List<Cage> cages)
        {
            Console.Write("Enter the Keeper ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int keeperId))
            {
                Console.WriteLine("Invalid Keeper ID.");
                return;
            }

            var keeper = keepers.Find(k => k.KeeperId == keeperId);
            if (keeper == null)
            {
                Console.WriteLine($"No keeper found with ID {keeperId}.");
                return;
            }

            // ✅ Unassign Keeper from Cages
            foreach (var cage in cages)
            {
                if (cage.ResponsibleKeeper != null && cage.ResponsibleKeeper.KeeperId == keeperId)
                {
                    cage.RemoveKeeper();
                }
            }

            // ✅ Mark Keeper as "Unassigned" instead of deleting
            keeper.Position = "Unassigned";
            keeper.AssignedCages.Clear(); // Remove cage assignments

            Console.WriteLine($"Keeper {keeper.FirstName} {keeper.LastName} has been unassigned from all cages.");
        }


        public bool AssignAnimalToCage(List<Animal> animals, List<Bird> birds, List<Cage> cages, string animalId, string cageId)
        {
            Console.WriteLine("Choose the type of entity to assign to a cage:");
            Console.WriteLine("1. Animal");
            Console.WriteLine("2. Bird");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter the Animal ID to assign to a cage: ");
                if (!int.TryParse(Console.ReadLine(), out int entityId))
                {
                    Console.WriteLine("Invalid Animal ID.");
                    return false;
                }

                var selectedAnimal = animals.Find(a => a.AnimalId == entityId);
                if (selectedAnimal == null)
                {
                    Console.WriteLine("Animal not found.");
                    return false;
                }

                Console.WriteLine("Available Cages:");
                foreach (var cage in cages)
                {
                    Console.WriteLine($"Cage Number: {cage.CageNumber}, Description: {cage.Description}");
                }

                Console.Write("Enter the Cage Number: ");
                if (!int.TryParse(Console.ReadLine(), out int cageNumber))
                {
                    Console.WriteLine("Invalid Cage Number.");
                    return false;
                }

                var selectedCage = cages.Find(c => c.CageNumber == cageNumber);
                if (selectedCage == null || !selectedCage.AssignAnimal(selectedAnimal))
                {
                    Console.WriteLine("Cage is not suitable for this animal.");
                    return false;
                }

                Console.WriteLine($"Animal '{selectedAnimal.Name}' has been assigned to Cage {cageNumber}.");
                return true;
            }
            else if (choice == "2")
            {
                Console.Write("Enter the Bird ID to assign to a cage: ");
                if (!int.TryParse(Console.ReadLine(), out int entityId))
                {
                    Console.WriteLine("Invalid Bird ID.");
                    return false;
                }
                // Example of a lambda expression used in the Find method:
                var selectedBird = birds.Find(b => b.BirdId == entityId);
                if (selectedBird == null)
                {
                    Console.WriteLine("Bird not found.");
                    return false;
                }

                Console.WriteLine("Available Cages:");
                foreach (var cage in cages)
                {
                    Console.WriteLine($"Cage Number: {cage.CageNumber}, Description: {cage.Description}");
                }

                Console.Write("Enter the Cage Number: ");
                if (!int.TryParse(Console.ReadLine(), out int cageNumber))
                {
                    Console.WriteLine("Invalid Cage Number.");
                    return false;
                }

                // A lambda expression is a concise way to represent an anonymous function.
                // It can be used to create delegates or expression tree types.
                // The syntax is: (input parameters) => expression or statement block

                // Example of a lambda expression used in the Find method:
                // This lambda expression is used to find a cage in the list of cages
                // It checks if the CageNumber of each cage matches the specified cageNumber
                var selectedCage = cages.Find(c => c.CageNumber == cageNumber);
                if (selectedCage == null || !selectedCage.AssignBird(selectedBird))
                {
                    Console.WriteLine("Cage is not suitable for this bird.");
                    return false;
                }

                Console.WriteLine($"Bird '{selectedBird.Name}' has been assigned to Cage {cageNumber}.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return false;
            }
        }

    }
}
