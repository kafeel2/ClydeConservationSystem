using System;
using System.Collections.Generic;
using System.Text;

namespace ClydeConservationSystem
{ /// <summary>
  /// Kafeel Ahmed
  /// Date: 2025/01/18
  /// Description: I have been asked to develop a program for the Clyde Conservation System that manages nimals, birds, cages, and keepers.
  /// </summary>

    public class Program
    {
        //private static Stream filePath;
        //private static string keepersFilePath;

        public static void Main(string[] args)
        {
            string filePath = "animals.txt";
            string loansFilePath = "loans.txt";
            string borrowedFilePath = "borrowed.txt";
            string keeperFilePath = "keepers.txt";
            string reportFilePath = "weeklyReport.txt";
            string cagesFilePath = "cages.txt";  // ✅ Added for cages
            string enclosuresFilePath = "enclosures.txt";  // ✅ Added for enclosures



            if (!File.Exists(loansFilePath))
            {
                File.Create(loansFilePath).Close(); // Create an empty file
                Console.WriteLine("loans.txt file created.");
            }


            // Ensure the file exists before any operations
            if (!File.Exists(filePath))
            {
                Console.WriteLine("animals.txt file is missing in the output directory. Ensure it is correctly set up.");
                return; // Prevent further operations if the file is missing
            }

            if (!File.Exists(keeperFilePath))
            {
                File.Create(keeperFilePath).Close(); // ✅ Create an empty keepers.txt if missing
                Console.WriteLine("keepers.txt file created.");
            }


            // Load animals from the file
            bool isRunning = true;
            List<Animal> animals = new List<Animal>();
            List<Bird> birds = new List<Bird>();
            List<Cage> cages = new List<Cage>();
            List<HeadKeeper> keepers = new List<HeadKeeper>();
            List<Loan> loans = new List<Loan>();
            List<Borrowed> borrowed = new List<Borrowed>();
            List<Enclosure> enclosures = new List<Enclosure>(); // ✅ Add t




            //Call the test method
            //TestAnimalFileOperations(animals, birds);

            // 1. Load cages from file FIRST
            LoadCagesFromFile(cages);

            // 2. If no cages were loaded, add default cages
            if (cages.Count == 0)
            {
                cages.Add(new Cage(1, "Large Predator Cage", 2));
                cages.Add(new Cage(2, "Small Herbivore Cage", 3));
                cages.Add(new Cage(3, "Reptile Cage", 1));
                Cage.SaveCagesToFile(cages); //
            }

            // 3. Load animals and birds from file
            LoadAnimalsFromFile(animals, birds, cages);

            // 4. Load keepers from file
            LoadKeepersFromFile(keepers, cages);

            // 5. Link animals and birds with their respective cages
            LinkCagesWithEntities(animals, birds, cages);

            // 6. Load loans from file ✅ Ensure correct file path is used
            LoadLoansFromFile(loans);

            // 7. Load borrowed animals and birds from file ✅ New step added
            LoadBorrowedFromFile(borrowed);

            // 8. Generate weekly report at startup (optional, for debugging) ✅ Includes borrowed animals now
            GenerateWeeklyReport(animals, birds, loans, borrowed, keepers);







            // Main menu loop

            while (isRunning)
            {
                Console.Clear(); // Clear the screen at the start of the loop
                Console.WriteLine("=================WELCOME======================");
                Console.WriteLine($"System Date: {DateTime.Now:yyyy-MM-dd}");
                Console.WriteLine($"Total Animals: {animals.Count} | Total Birds: {birds.Count}");
                Console.WriteLine("*******************************************\n");

                Console.WriteLine("=============================================");
                Console.WriteLine("1. Add an Animal");
                Console.WriteLine("2. Allocate an Animal to a Cage");
                Console.WriteLine("3. Allocate a Keeper to a Cage");
                Console.WriteLine("4. View Animal Details");
                Console.WriteLine("5. View Keeper and Cage Allocations");
                Console.WriteLine("6. View Cage Details");
                Console.WriteLine("7. Generate Weekly Reports");
                Console.WriteLine("8. Loan Out an Animal");
                Console.WriteLine("9. Borrow an Animal");
                Console.WriteLine("10. Delete an Animal");
                Console.WriteLine("11. Add a Keeper");
                Console.WriteLine("12. Exit");


                Console.WriteLine("=============================================");
                Console.Write("\nEnter your choice: ");

                // Get the user's choice
                string choice = Console.ReadLine();
                Console.WriteLine($"You selected: {choice}. Processing...\n");

                switch (choice)
                {
                    case "1": // Add Animal
                        AddAnimal(animals, birds, cages);
                        SaveAnimalsToFile(animals, birds); // Save immediately after adding an animal or bird
                        break;

                    case "2": // Assign an Animal to a Cage
                        Console.Write("Enter Animal ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int animalId))
                        {
                            Console.WriteLine("Invalid Animal ID. Please enter a numeric value.");
                            break;
                        }

                        Console.Write("Enter Cage ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int cageId))
                        {
                            Console.WriteLine("Invalid Cage ID. Please enter a numeric value.");
                            break;
                        }

                        // Find the animal or bird
                        var animalToAssign = animals.FirstOrDefault(a => a.AnimalId == animalId);
                        var birdToAssign = birds.FirstOrDefault(b => b.BirdId == animalId);

                        if (animalToAssign == null && birdToAssign == null)
                        {
                            Console.WriteLine($"No animal or bird found with ID {animalId}.");
                            break;
                        }

                        // ✅ Check if the cage exists
                        var targetCage = cages.FirstOrDefault(c => c.CageNumber == cageId);

                        if (targetCage == null)
                        {
                            // ✅ Check if the cage was REMOVED via Option 6
                            bool wasRemoved = !File.ReadAllLines(@"..\..\..\cages.txt").Any(line => line.StartsWith($"{cageId},"));

                            if (wasRemoved)
                            {
                                Console.WriteLine($"❌ Cage {cageId} was removed and cannot be used again.");
                                Console.Write("Would you like to create a new cage for this animal? (Y/N): ");
                                string createCageChoice = Console.ReadLine();

                                if (!createCageChoice.Equals("Y", StringComparison.OrdinalIgnoreCase))
                                {
                                    Console.WriteLine("Action cancelled. Animal not assigned.");
                                    break;
                                }

                                // ✅ Create a new cage and allow assignment
                                Console.Write("Enter a description for the new cage: ");
                                string cageDescription = Console.ReadLine();

                                Console.Write("Enter the maximum number of animals allowed in the cage: ");
                                if (!int.TryParse(Console.ReadLine(), out int maxAnimals) || maxAnimals < 1)
                                {
                                    Console.WriteLine("Invalid input. Defaulting to max capacity of 5.");
                                    maxAnimals = 5;
                                }

                                targetCage = new Cage(cageId, cageDescription, maxAnimals);
                                cages.Add(targetCage);
                                Cage.SaveCagesToFile(cages); // ✅ Save newly created cage
                            }
                            else
                            {
                                Console.WriteLine($"⚠️ Cage {cageId} does not exist. Recreating...");
                                targetCage = new Cage(cageId, $"Recovered Cage {cageId}", 5); // Default max capacity: 5
                                cages.Add(targetCage);
                                Cage.SaveCagesToFile(cages); // ✅ Ensure cages are saved
                            }
                        }

                        // ✅ Check if the cage is suitable before assigning
                        if (animalToAssign != null)
                        {
                            if (!targetCage.CheckSuitability(animalToAssign))
                            {
                                Console.WriteLine($"Cage {cageId} is not suitable for the animal '{animalToAssign.Name}' ({animalToAssign.Type}).");
                                break;
                            }
                        }
                        else if (birdToAssign != null)
                        {
                            if (!targetCage.CheckSuitability(birdToAssign))
                            {
                                Console.WriteLine($"Cage {cageId} is not suitable for the bird '{birdToAssign.Name}' ({birdToAssign.Type}).");
                                break;
                            }
                        }

                        // ✅ Remove animal from previous cage before assigning a new one
                        foreach (var cage in cages)
                        {
                            if (cage.AssignedAnimals.Contains(animalToAssign))
                            {
                                cage.AssignedAnimals.Remove(animalToAssign);
                            }
                            if (cage.AssignedBirds.Contains(birdToAssign))
                            {
                                cage.AssignedBirds.Remove(birdToAssign);
                            }
                        }

                        // ✅ Assign the animal to the new cage
                        if (animalToAssign != null)
                        {
                            targetCage.AssignAnimal(animalToAssign);
                            animalToAssign.CageNumber = cageId; // ✅ Update the animal's cage number
                        }
                        else if (birdToAssign != null)
                        {
                            targetCage.AssignBird(birdToAssign);
                            birdToAssign.CageNumber = cageId; // ✅ Update the bird's cage number
                        }

                        Console.WriteLine($"✅ Successfully assigned to Cage {cageId}.");
                        SaveAnimalsToFile(animals, birds); // ✅ Save updated animal cage assignments
                        Cage.SaveCagesToFile(cages); // ✅ Save updated cages
                        break;




                    case "3": // Assign a Keeper to a Cage
                        Console.Write("Enter Keeper ID: ");
                        if (!int.TryParse(Console.ReadLine(), out int keeperId))
                        {
                            Console.WriteLine("Invalid Keeper ID. Please enter a numeric value.");
                            break;
                        }

                        // Find the keeper
                        HeadKeeper selectedKeeper = keepers.Find(k => k.KeeperId == keeperId);
                        if (selectedKeeper == null)
                        {
                            Console.WriteLine($"No keeper found with ID {keeperId}.");
                            break;
                        }

                        // Call the AssignKeeperToCage method
                        if (selectedKeeper.AssignKeeperToCage(cages))
                        {
                            Console.WriteLine("✅ Keeper assigned to cage successfully.");

                            // ✅ Save updated keepers list to file so it persists
                            SaveKeepersToFile(keepers);
                        }
                        else
                        {
                            Console.WriteLine("❌ Failed to assign keeper to a cage.");
                        }
                        break;



                    case "4": // View Animal Details
                        ViewAnimalDetails(animals, birds);
                        break;

                    case "5": // View Keeper and Cage Allocations
                        ViewKeeperAndCageAllocations(keepers, cages);
                        break;

                    case "6": // View Cage Details
                        ViewCageDetails(cages, animals, birds); // ✅ Passes required arguments
                        Console.Write("\nDo you want to remove a cage? (Y/N): ");
                        string removeChoice = Console.ReadLine();

                        if (removeChoice.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            Enclosure.RemoveCage(cages, animals, birds); // ✅ Pass animals & birds lists
                            Cage.SaveCagesToFile(cages);  // ✅ Ensure cages are saved after removal
                        }
                        break;



                    case "7": // Generate Weekly Report
                        GenerateWeeklyReport(animals, birds, loans, borrowed, keepers); // ✅ Generate and display the report
                        Console.WriteLine("Weekly report updated.");
                        break;


                    case "8": // Loan Out Animal
                        LoanOutEntity(animals, birds, loans);
                        SaveLoansToFile(loans); //CS7036 There is no argument given that corresponds to the required parameter 'loansFilePath' of 'Program.SaveLoansToFile(List<Loan>,string)'
                        break;

                    case "9": // Borrow an Animal or Bird
                        BorrowEntity(animals, birds, borrowed);
                        SaveBorrowedToFile(borrowed);
                        Console.WriteLine("Borrowed entity successfully recorded.");
                        break;



                    case "10": // Delete Animal
                        DeleteAnimal(animals, birds, cages, keepers); // CS1501 No overload for method 'DeleteAnimal' takes 3 arguments
                        SaveAnimalsToFile(animals, birds);
                        break;


                    case "11": // Add or Remove Keeper
                          Console.WriteLine("1. Add Keeper");
                         Console.WriteLine("2. Remove Keeper");
                         Console.Write("Enter choice: ");
                            string keeperChoice = Console.ReadLine();

                            if (keeperChoice == "1")
                            {
                                var headKeeper = new HeadKeeper();
                                SaveKeepersToFile(keepers); // ✅ Save keepers to file after adding
                                SaveAnimalsToFile(animals, birds); // ✅ Save immediately
                                Console.WriteLine("Keeper information saved to file.");
                            }
                            else if (keeperChoice == "2")
                            {
                                HeadKeeper.RemoveKeeper(keepers, cages); // ✅ Call method from HeadKeeper.cs
                                SaveKeepersToFile(keepers); // ✅ Save changes to file after removal
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice.");
                            }
                          break;
                    case "12": // Exit
                        Console.Write("Are you sure you want to exit? (Y/N): ");
                        string confirmExit = Console.ReadLine();
                        if (confirmExit.Equals("Y", StringComparison.OrdinalIgnoreCase))
                        {
                            isRunning = false;
                        }
                        else
                        {
                            Console.WriteLine("Returning to the main menu...");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }




                // Prompt to return to the main menu or exit
                if (isRunning) // Only show this if the user hasn't chosen to exit
                {
                    Console.WriteLine("\nPress 'M' to return to the main menu, or any other key to exit...");
                    string returnChoice = Console.ReadLine();
                    if (!returnChoice.Equals("M", StringComparison.OrdinalIgnoreCase))
                    {
                        isRunning = false; // Exit the loop
                    }
                }
            }

        }

        // Method to add an animal to the system
        static void AddAnimal(List<Animal> animals, List<Bird> birds, List<Cage> cages)
        {
            Console.WriteLine("Choose the type of animal to add:");
            Console.WriteLine("1. Mammal");
            Console.WriteLine("2. Reptile");
            Console.WriteLine("3. Flying Bird");
            Console.WriteLine("4. Non-Flying Bird");
            Console.WriteLine("\n");
            Console.Write("Enter your choice: ");

            string typeChoice = Console.ReadLine();
            switch (typeChoice)
            {
                case "1":
                    AddMammal(animals, cages);
                    break;
                case "2":
                    AddReptile(animals, cages);
                    break;
                case "3":
                    AddFlyingBird(birds, cages);
                    break;
                case "4":
                    AddNonFlyingBird(birds, cages);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Returning to main menu.");
                    break;
            }
        }


        // Method to add a mammal to the system
        // Method to add a mammal to the system
        public static void AddMammal(List<Animal> animals, List<Cage> cages)
        {
            Console.WriteLine("Adding a Mammal...");
            Console.WriteLine("Enter Animal ID:");

            int animalId;
            if (!int.TryParse(Console.ReadLine(), out animalId))
            {
                Console.WriteLine("❌ Invalid ID. Please enter a numeric value.");
                return;
            }

            var existingAnimal = animals.FirstOrDefault(a => a.AnimalId == animalId);
            if (existingAnimal != null)
            {
                Console.WriteLine($"❌ An animal with ID {animalId} already exists. Name: {existingAnimal.Name}, Type: {existingAnimal.GetType().Name}");
                return;
            }

            Console.WriteLine("Enter the species (e.g., lion, tiger): ");
            string type = Console.ReadLine();
            Console.WriteLine("Enter the name of the animal: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the date of birth of the animal (YYYY-MM-DD): ");
            string dateOfBirth = Console.ReadLine();
            Console.WriteLine("Enter the date of acquisition of the animal (YYYY-MM-DD): ");
            string dateOfAcquisition = Console.ReadLine();
            Console.WriteLine("Enter the danger rating of the animal (1-5): ");
            int dangerRating = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter the cage number allocated to the animal:");
            int cageNumber = Convert.ToInt32(Console.ReadLine());

            // ✅ Check if cage exists, if not, create it automatically
            var targetCage = cages.FirstOrDefault(c => c.CageNumber == cageNumber);
            if (targetCage == null)
            {
                Console.WriteLine($"⚠️ Cage {cageNumber} does not exist. Creating a new cage...");
                targetCage = new Cage(cageNumber, $"Cage {cageNumber}", 5); // Default description & max animals
                cages.Add(targetCage); // Add the new cage
                Cage.SaveCagesToFile(cages); // ✅ Save cages immediately
            }

            Console.WriteLine("Enter the insurance value of the animal: ");
            double insuranceValue = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the diet of the animal: ");
            string diet = Console.ReadLine();
            Console.WriteLine("Enter the habitat of the animal: ");
            string habitat = Console.ReadLine();
            Console.WriteLine("Enter the sex of the animal (Male/Female): ");
            string sex = Console.ReadLine();
            Console.WriteLine("Enter the mate's name (if any): ");
            string mateName = Console.ReadLine();
            string? lastBirthDate = null;

            if (sex.Equals("Female", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Has the mammal given birth? (Y/N): ");
                string hasGivenBirth = Console.ReadLine();
                if (hasGivenBirth.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Enter the date of the last birth (YYYY-MM-DD): ");
                    lastBirthDate = Console.ReadLine();
                }
            }

            Mammal mammal = new Mammal(animalId, name, type, dateOfBirth, dateOfAcquisition, dangerRating, cageNumber,
                                       insuranceValue, diet, habitat, sex, mateName, lastBirthDate);

            if (!targetCage.CheckSuitability(mammal))
            {
                Console.WriteLine($"❌ Cage {cageNumber} is not suitable for {type}.");
                return;
            }

            targetCage.AssignAnimal(mammal);
            animals.Add(mammal);
            Console.WriteLine($"✅ Mammal '{name}' added successfully to Cage {cageNumber}!");

            // ✅ Save the changes after adding an animal
            SaveAnimalsToFile(animals, new List<Bird>()); // Save animals
            Cage.SaveCagesToFile(cages); // Save cages
        }

        //  Method to view the details of all animals and birds
        static void ViewAnimalDetails(List<Animal> animals, List<Bird> birds)
        {
            Console.WriteLine("\n--- Viewing All Animal Details ---");

            if (animals.Count == 0 && birds.Count == 0)
            {
                Console.WriteLine("No animals or birds have been added to the system yet.");
                return;
            }

            // Display details for Animals
            if (animals.Count > 0)
            {
                Console.WriteLine("\n--- Animals ---");
                foreach (var animal in animals)
                {
                    animal.PrintDetails();
                    Console.WriteLine("===========================================================================");

                }
            }

            // Display details for Birds
            if (birds.Count > 0)
            {
                Console.WriteLine("\n--- Birds ---");
                foreach (var bird in birds)
                {
                    bird.PrintDetails();
                    Console.WriteLine("===========================================================================");

                }
            }
        }

        static void ViewCageDetails(List<Cage> cages, List<Animal> animals, List<Bird> birds)
        {
            Console.WriteLine("\n--- Viewing Cage Details ---");

            // ✅ Identify cages that exist in animal/bird records but are missing from cages list
            var missingCages = animals.Select(a => a.CageNumber)
                                      .Concat(birds.Select(b => b.CageNumber))
                                      .Distinct()
                                      .Where(cageNumber => cageNumber >= 1 && cageNumber <= 8) // ✅ Prevents out-of-range errors
                                      .Where(cageNumber => !cages.Any(c => c.CageNumber == cageNumber))
                                      .ToList();

            // ✅ Automatically add missing cages
            foreach (var cageNumber in missingCages)
            {
                Console.WriteLine($"⚠️ Cage {cageNumber} exists in animal records but is missing from the cages list. Re-adding...");
                cages.Add(new Cage(cageNumber, $"Cage {cageNumber} (Recovered)", 5)); // Default description & max animals
            }

            if (cages.Count == 0)
            {
                Console.WriteLine("No cages have been added to the system.");
                return;
            }

            foreach (var cage in cages)
            {
                cage.ShowCageDetails();
                Console.WriteLine();
            }

            // ✅ Save cages after recovery
            Cage.SaveCagesToFile(cages);
        }




        // Method to view the details of all cages
        static void ViewKeeperAndCageAllocations(List<HeadKeeper> keepers, List<Cage> cages)
        {
            Console.WriteLine("\n--- Viewing Keeper and Cage Allocations ---");

            if (keepers.Count == 0)
            {
                Console.WriteLine("No keepers have been added to the system.");
                return;
            }

            foreach (var keeper in keepers)
            {
                Console.WriteLine($"Keeper ID: {keeper.KeeperId}, Name: {keeper.FirstName} {keeper.LastName}");
                if (keeper.AssignedCages.Count > 0)
                {
                    Console.WriteLine("Allocated Cages:");
                    foreach (var cageNumber in keeper.AssignedCages)
                    {
                        Console.WriteLine($"- Cage Number: {cageNumber}");
                    }
                }
                else
                {
                    Console.WriteLine("No cages allocated.");
                }
                Console.WriteLine();
            }
        }


        
        // Method to generate weekly reports
        static void GenerateWeeklyReport(List<Animal> animals, List<Bird> birds, List<Loan> loans, List<Borrowed> borrowed, List<HeadKeeper> keepers)

        {
            string reportPath = @"..\..\..\weeklyReport.txt"; // ✅ Ensure correct relative path
            StringBuilder reportContent = new StringBuilder();

            reportContent.AppendLine("=== Weekly Report ===");
            reportContent.AppendLine($"Date Generated: {DateTime.Now:yyyy-MM-dd}");
            reportContent.AppendLine("==========================================");

            // 🦁 Animal Summary
            reportContent.AppendLine("\n--- Animal Summary ---");
            foreach (var animal in animals)
            {
                reportContent.AppendLine($"ID: {animal.AnimalId}, Name: {animal.Name}, Type: {animal.Type}, Cage: {animal.CageNumber}");
            }

            // 🦜 Bird Summary
            reportContent.AppendLine("\n--- Bird Summary ---");
            foreach (var bird in birds)
            {
                reportContent.AppendLine($"ID: {bird.BirdId}, Name: {bird.Name}, Type: {bird.Type}, Cage: {bird.CageNumber}");
            }

            // 📅 Loaning and Borrowing Activity (Last 7 Days)
            reportContent.AppendLine("\n--- Loaning and Borrowing Activity ---");
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

            // Loaned-Out Animals and Birds
            reportContent.AppendLine("\n--- Loaned-Out Animals & Birds ---");
            foreach (var loan in loans.Where(l => DateTime.Parse(l.DateOut) >= oneWeekAgo))
            {
                string entityType = loan.IsAnimal ? "Animal" : "Bird";
                reportContent.AppendLine($"Loan ID: {loan.LoanId}, {entityType} ID: {loan.EntityId}, Establishment: {loan.EstablishmentDetails}, Date Out: {loan.DateOut}, Date Returned: {loan.DateBackIn ?? "Not Yet Returned"}");
            }

            // Borrowed Animals and Birds
            reportContent.AppendLine("\n--- Borrowed Animals & Birds ---");
            foreach (var item in borrowed.Where(b => DateTime.Parse(b.DateOut) >= oneWeekAgo))
            {
                string entityType = item.IsAnimal ? "Animal" : "Bird";
                reportContent.AppendLine($"Borrowed ID: {item.LoanId}, {entityType} ID: {item.EntityId}, Borrowed From: {item.BorrowedFrom}, Date Borrowed: {item.DateOut}, Date Returned: {item.DateBackIn ?? "Not Yet Returned"}");
            }

            // 🔹 Keeper Allocations
            reportContent.AppendLine("\n--- Keeper Allocations ---");
            foreach (var keeper in keepers)
            {
                reportContent.AppendLine($"Keeper ID: {keeper.KeeperId}, Name: {keeper.FirstName} {keeper.LastName}, Assigned Cages: {string.Join(", ", keeper.AssignedCages)}");
            }

            // 🏥 Health Checks and Vet Visits (Placeholder)
            reportContent.AppendLine("\n--- Health Checks and Vet Visits ---");
            reportContent.AppendLine("Health checks and vet visit information to be implemented...");

            // ✅ Save Report to File
            File.WriteAllText(reportPath, reportContent.ToString(), Encoding.UTF8);
            Console.WriteLine($"✅ Weekly report generated at: {Path.GetFullPath(reportPath)}");

            // 📜 Display the Report in Console
            Console.WriteLine("\n--- Weekly Report Preview ---");
            Console.WriteLine(reportContent.ToString());
        }

        // Method to delete an animal or bird
        public static void DeleteAnimal(List<Animal> animals, List<Bird> birds, List<Cage> cages, List<HeadKeeper> keepers)
        {
            Console.WriteLine("\n--- Delete an Animal ---");

            if (animals.Count == 0 && birds.Count == 0)
            {
                Console.WriteLine("No animals or birds are available to delete.");
                return;
            }

            Console.WriteLine("Enter the ID of the animal or bird you want to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int idToDelete))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric value.");
                return;
            }

            // Search for the animal in the animals list
            var animalToDelete = animals.Find(a => a.AnimalId == idToDelete);
            if (animalToDelete != null)
            {
                animals.Remove(animalToDelete);
                var cage = cages.FirstOrDefault(c => c.AssignedAnimals.Contains(animalToDelete));
                if (cage != null)
                {
                    cage.AssignedAnimals.Remove(animalToDelete);
                    Console.WriteLine($"DEBUG: Animal '{animalToDelete.Name}' removed from Cage {cage.CageNumber}.");
                    cage.SaveCageDetailsToFile();

                    // Attempt to reassign the animal if necessary
                    if (cage.AssignAnimal(animalToDelete))
                    {
                        SaveAnimalsToFile(animals, birds); // Update file after moving
                        Console.WriteLine($"Animal '{animalToDelete.Name}' moved to Cage {cage.CageNumber}.");
                    }
                }

                Console.WriteLine($"Animal with ID {idToDelete} has been deleted.");
                return;
            }

            // Search for the bird in the birds list
            var birdToDelete = birds.Find(b => b.BirdId == idToDelete);
            if (birdToDelete != null)
            {
                birds.Remove(birdToDelete);
                var cage = cages.FirstOrDefault(c => c.AssignedBirds.Contains(birdToDelete));
                if (cage != null)
                {
                    cage.AssignedBirds.Remove(birdToDelete);
                    Console.WriteLine($"DEBUG: Bird '{birdToDelete.Name}' removed from Cage {cage.CageNumber}.");
                    cage.SaveCageDetailsToFile();

                    // Attempt to reassign the bird if necessary
                    if (cage.AssignBird(birdToDelete))
                    {
                        SaveAnimalsToFile(animals, birds); // Update file after moving
                        Console.WriteLine($"Bird '{birdToDelete.Name}' moved to Cage {cage.CageNumber}.");
                    }
                }

                Console.WriteLine($"Bird with ID {idToDelete} has been deleted.");
                return;
            }

            Console.WriteLine($"No animal or bird found with ID {idToDelete}.");
        }




        // Method to add a reptile to the system
        public static void AddReptile(List<Animal> animals, List<Cage> cages)
        {
            Console.WriteLine("Adding a Reptile...");
            Console.WriteLine("Enter Reptile ID:");

            int animalId;
            if (!int.TryParse(Console.ReadLine(), out animalId))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric value.");
                return;
            }

            var existingAnimal = animals.FirstOrDefault(a => a.AnimalId == animalId);
            if (existingAnimal != null)
            {
                Console.WriteLine($"A reptile with ID {animalId} already exists. Name: {existingAnimal.Name}, Type: {existingAnimal.GetType().Name}");
                return;
            }

            Console.WriteLine("Enter the species (e.g., snake, crocodile): ");
            string type = Console.ReadLine();
            Console.WriteLine("Enter the name of the reptile: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the date of birth of the reptile (YYYY-MM-DD): ");
            string dateOfBirth = Console.ReadLine();
            Console.WriteLine("Enter the date of acquisition of the reptile (YYYY-MM-DD): ");
            string dateOfAcquisition = Console.ReadLine();
            Console.WriteLine("Enter the danger rating of the reptile (1-5): ");
            int dangerRating = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the cage number allocated to the reptile:");
            int cageNumber = Convert.ToInt32(Console.ReadLine());

            var targetCage = cages.FirstOrDefault(c => c.CageNumber == cageNumber);
            if (targetCage == null)
            {
                Console.WriteLine($"Cage {cageNumber} does not exist.");
                return;
            }

            Console.WriteLine("Enter the insurance value of the reptile: ");
            double insuranceValue = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the diet of the reptile: ");
            string diet = Console.ReadLine();
            Console.WriteLine("Enter the habitat of the reptile: ");
            string habitat = Console.ReadLine();
            Console.WriteLine("Enter the sex of the reptile (Male/Female): ");
            string sex = Console.ReadLine();
            Console.WriteLine("Enter the tank temperature of the reptile: ");
            double tankTemperature = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the environment of the reptile (e.g., sand, water): ");
            string environment = Console.ReadLine();

            Reptile reptile = new Reptile(animalId, name, type, dateOfBirth, dateOfAcquisition, dangerRating, cageNumber,
                                          insuranceValue, diet, habitat, sex, tankTemperature, environment);

            if (!targetCage.CheckSuitability(reptile))
            {
                Console.WriteLine($"Cage {cageNumber} is not suitable for {type}.");
                return;
            }

            targetCage.AssignAnimal(reptile);
            animals.Add(reptile);
            Console.WriteLine($"Reptile '{name}' added successfully to Cage {cageNumber}!");
        }




        // Method to add a flying bird to the system
        public static void AddFlyingBird(List<Bird> birds, List<Cage> cages)
        {
            Console.WriteLine("Adding a Flying Bird...");
            Console.WriteLine("Enter Bird ID:");

            if (!int.TryParse(Console.ReadLine(), out int birdId))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric value.");
                return;
            }

            var existingBird = birds.FirstOrDefault(b => b.BirdId == birdId);
            if (existingBird != null)
            {
                Console.WriteLine($"A bird with ID {birdId} already exists. Name: {existingBird.Name}, Type: {existingBird.GetType().Name}");
                return;
            }

            Console.WriteLine("Enter the species (e.g., eagle, hawk): ");
            string type = Console.ReadLine();
            Console.WriteLine("Enter the name of the bird: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the date of birth of the bird (YYYY-MM-DD): ");
            string dateOfBirth = Console.ReadLine();
            Console.WriteLine("Enter the danger rating of the bird (1-5): ");
            int dangerRating = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the cage number allocated to the bird:");
            int cageNumber = Convert.ToInt32(Console.ReadLine());

            var targetCage = cages.FirstOrDefault(c => c.CageNumber == cageNumber);
            if (targetCage == null)
            {
                Console.WriteLine($"Cage {cageNumber} does not exist.");
                return;
            }

            Console.WriteLine("Enter the wing span of the bird (meters): ");
            double wingSpan = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the flight speed of the bird (km/h): ");
            double flightSpeed = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the nest environment of the bird (e.g., tree, cliff): ");
            string nestEnvironment = Console.ReadLine();
            Console.WriteLine("Enter the feeding requirement of the bird: ");
            string feedingRequirement = Console.ReadLine();
            Console.WriteLine("Enter the diet of the bird (e.g., seeds, insects): ");
            string diet = Console.ReadLine();
            Console.WriteLine("Enter the habitat of the bird (e.g., forest, wetland): ");
            string habitat = Console.ReadLine();
            Console.WriteLine("Enter the sex of the bird (Male/Female): ");
            string sex = Console.ReadLine();
            Console.WriteLine("Enter the insurance value of the bird: ");
            double insuranceValue = Convert.ToDouble(Console.ReadLine());

            FlyingBird flyingBird = new FlyingBird(
                birdId, name, type, dateOfBirth, dangerRating, insuranceValue, diet, habitat, sex, feedingRequirement, nestEnvironment, wingSpan, flightSpeed, cageNumber
            );

            if (!targetCage.CheckSuitability(flyingBird))
            {
                Console.WriteLine($"Cage {cageNumber} is not suitable for {type}.");
                return;
            }

            targetCage.AssignBird(flyingBird);
            birds.Add(flyingBird);
            Console.WriteLine($"Flying Bird '{name}' added successfully to Cage {cageNumber}!");
        }




        // Method to add a non-flying bird to the system
        public static void AddNonFlyingBird(List<Bird> birds, List<Cage> cages)
        {
            Console.WriteLine("Adding a Non-Flying Bird...");
            Console.WriteLine("Enter Bird ID:");

            int birdId;
            if (!int.TryParse(Console.ReadLine(), out birdId))
            {
                Console.WriteLine("Invalid ID. Please enter a numeric value.");
                return;
            }

            var existingBird = birds.FirstOrDefault(b => b.BirdId == birdId);
            if (existingBird != null)
            {
                Console.WriteLine($"A bird with ID {birdId} already exists. Name: {existingBird.Name}, Type: {existingBird.GetType().Name}");
                return;
            }

            Console.WriteLine("Enter the species (e.g., penguin, ostrich): ");
            string type = Console.ReadLine();
            Console.WriteLine("Enter the name of the bird: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the date of birth of the bird (YYYY-MM-DD): ");
            string dateOfBirth = Console.ReadLine();
            Console.WriteLine("Enter the danger rating of the bird (1-5): ");
            int dangerRating = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the cage number allocated to the bird:");
            int cageNumber = Convert.ToInt32(Console.ReadLine());

            var targetCage = cages.FirstOrDefault(c => c.CageNumber == cageNumber);
            if (targetCage == null)
            {
                Console.WriteLine($"Cage {cageNumber} does not exist.");
                return;
            }

            Console.WriteLine("Enter the land speed of the bird (km/h): ");
            double landSpeed = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the preferred habitat of the bird (e.g., land, water): ");
            string preferredHabitat = Console.ReadLine();
            Console.WriteLine("Enter the insurance value of the bird: ");
            double insuranceValue = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter the diet of the bird: ");
            string diet = Console.ReadLine();
            Console.WriteLine("Enter the habitat of the bird: ");
            string habitat = Console.ReadLine();
            Console.WriteLine("Enter the sex of the bird (Male/Female): ");
            string sex = Console.ReadLine();
            Console.WriteLine("Enter the feeding requirement of the bird: ");
            string feedingRequirement = Console.ReadLine();
            Console.WriteLine("Enter the nest environment of the bird (e.g., tree, cliff): ");
            string nestEnvironment = Console.ReadLine();

            NonFlyingBird nonFlyingBird = new NonFlyingBird(
                birdId, name, type, dateOfBirth, dangerRating, insuranceValue, diet,
                habitat, sex, feedingRequirement, nestEnvironment, landSpeed, preferredHabitat, cageNumber);

            if (!targetCage.CheckSuitability(nonFlyingBird))
            {
                Console.WriteLine($"Cage {cageNumber} is not suitable for {type}.");
                return;
            }

            targetCage.AssignBird(nonFlyingBird);
            birds.Add(nonFlyingBird);
            Console.WriteLine($"Non-Flying Bird '{name}' added successfully to Cage {cageNumber}!");
        }





        // Method to save animals and birds to a file
        public static void SaveAnimalsToFile(List<Animal> animals, List<Bird> birds)

        {
            string animalsFilePath = @"..\..\..\animals.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(animalsFilePath))
                {
                    foreach (var animal in animals)
                    {
                        if (animal is Mammal mammal)
                        {
                            writer.WriteLine($"Mammal,{mammal.AnimalId},{mammal.Name},{mammal.Type},{mammal.DateOfBirth},{mammal.DateOfAcquisition},{mammal.DangerRating},{mammal.CageNumber},{mammal.InsuranceValue},{mammal.Diet},{mammal.Habitat},{mammal.Sex},{mammal.MateName},{mammal.BirthDate ?? "N/A"}");
                        }
                        else if (animal is Reptile reptile)
                        {
                            writer.WriteLine($"Reptile,{reptile.AnimalId},{reptile.Name},{reptile.Type},{reptile.DateOfBirth},{reptile.DateOfAcquisition},{reptile.DangerRating},{reptile.CageNumber},{reptile.InsuranceValue},{reptile.Diet},{reptile.Habitat},{reptile.Sex},{reptile.TankTemperature},{reptile.Environment}");
                        }
                    }

                    foreach (var bird in birds)
                    {
                        if (bird is FlyingBird flyingBird)
                        {
                            writer.WriteLine($"FlyingBird,{flyingBird.BirdId},{flyingBird.Name},{flyingBird.Type},{flyingBird.DateOfBirth},{flyingBird.DangerRating},{flyingBird.InsuranceValue},{flyingBird.Diet},{flyingBird.Habitat},{flyingBird.Sex},{flyingBird.FeedingRequirement},{flyingBird.NestEnvironment},{flyingBird.WingSpan},{flyingBird.FlightSpeed},{flyingBird.CageNumber}");
                        }
                        else if (bird is NonFlyingBird nonFlyingBird)
                        {
                            writer.WriteLine($"NonFlyingBird,{nonFlyingBird.BirdId},{nonFlyingBird.Name},{nonFlyingBird.Type},{nonFlyingBird.DateOfBirth},{nonFlyingBird.DangerRating},{nonFlyingBird.InsuranceValue},{nonFlyingBird.Diet},{nonFlyingBird.Habitat},{nonFlyingBird.Sex},{nonFlyingBird.FeedingRequirement},{nonFlyingBird.NestEnvironment},{nonFlyingBird.LandSpeed},{nonFlyingBird.PreferredHabitat},{nonFlyingBird.CageNumber}");
                        }
                    }
                }
                Console.WriteLine("Animal data successfully saved to animals.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving animal data: {ex.Message}");
            }
        }

        public static void SaveKeepersToFile(List<HeadKeeper> keepers)
        {
            string keepersFilePath = @"..\..\..\keepers.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(keepersFilePath))
                {
                    foreach (var keeper in keepers)
                    {
                       

                        string assignedCages = keeper.AssignedCages.Count > 0 ? string.Join("|", keeper.AssignedCages) : "N/A";
                        writer.WriteLine($"HeadKeeper,{keeper.KeeperId},{keeper.FirstName},{keeper.LastName},{keeper.Position},{assignedCages}");

                    }
                }
                Console.WriteLine("Keeper data successfully saved to keepers.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving keeper data: {ex.Message}");
            }
        }

        static void LoadCagesFromFile(List<Cage> cages)
        {
            string cagesFilePath = @"..\..\..\cages.txt";

            if (!File.Exists(cagesFilePath))
            {
                Console.WriteLine($"⚠️ cages.txt not found. Starting with an empty list.");
                return;
            }

            try
            {
                cages.Clear(); // ✅ Clears the list before reloading to prevent duplicates

                using (StreamReader reader = new StreamReader(cagesFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        string[] dataArray = line.Split(',');

                        int cageNumber = int.Parse(dataArray[0]);
                        string description = dataArray[1];
                        int maxAnimals = int.Parse(dataArray[2]);
                        bool isActive = dataArray[3] == "Active"; // ✅ Only load active cages

                        if (isActive)
                        {
                            cages.Add(new Cage(cageNumber, description, maxAnimals));
                        }
                    }
                }
                Console.WriteLine("✅ Cages loaded successfully from cages.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading cages: {ex.Message}");
            }
        }


        //  Method to load animals and birds from a file
        static void LoadAnimalsFromFile(List<Animal> animals, List<Bird> birds, List<Cage> cages)
        {
            string filePath = @"..\..\..\animals.txt";

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at: {Path.GetFullPath(filePath)}. Starting with an empty list.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] dataArray = line.Split(',');

                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            dataArray[i] = dataArray[i].Trim();
                        }

                        string type = dataArray[0].ToLower();

                        switch (type)
                        {
                            case "mammal":
                                var mammal = new Mammal(
                                    Convert.ToInt32(dataArray[1]),
                                    dataArray[2], dataArray[3], dataArray[4], dataArray[5],
                                    Convert.ToInt32(dataArray[6]), Convert.ToInt32(dataArray[7]),
                                    Convert.ToDouble(dataArray[8]), dataArray[9], dataArray[10],
                                    dataArray[11], dataArray.Length > 12 ? dataArray[12] : "None",
                                    dataArray.Length > 13 ? dataArray[13] : null
                                );
                                animals.Add(mammal);
                                break;

                            case "reptile":
                                var reptile = new Reptile(
                                    Convert.ToInt32(dataArray[1]),
                                    dataArray[2], dataArray[3], dataArray[4], dataArray[5],
                                    Convert.ToInt32(dataArray[6]), Convert.ToInt32(dataArray[7]),
                                    Convert.ToDouble(dataArray[8]), dataArray[9], dataArray[10],
                                    dataArray[11], dataArray.Length > 12 ? Convert.ToDouble(dataArray[12]) : 0.0,
                                    dataArray.Length > 13 ? dataArray[13] : "Unknown"
                                );
                                animals.Add(reptile);
                                break;

                            case "flyingbird":
                                var flyingBird = new FlyingBird(
                                    Convert.ToInt32(dataArray[1]),
                                    dataArray[2], dataArray[3], dataArray[4],
                                    Convert.ToInt32(dataArray[5]), Convert.ToDouble(dataArray[6]),
                                    dataArray[7], dataArray[8], dataArray[9], dataArray[10],
                                    dataArray[11], dataArray.Length > 12 ? Convert.ToDouble(dataArray[12]) : 1.0,
                                    dataArray.Length > 13 ? Convert.ToDouble(dataArray[13]) : 10.0,
                                    Convert.ToInt32(dataArray[14])
                                );
                                birds.Add(flyingBird);
                                break;

                            case "nonflyingbird":
                                var nonFlyingBird = new NonFlyingBird(
                                    Convert.ToInt32(dataArray[1]),
                                    dataArray[2], dataArray[3], dataArray[4],
                                    Convert.ToInt32(dataArray[5]), Convert.ToDouble(dataArray[6]),
                                    dataArray[7], dataArray[8], dataArray[9], dataArray[10],
                                    dataArray[11], dataArray.Length > 12 ? Convert.ToDouble(dataArray[12]) : 1.0,
                                    dataArray.Length > 13 ? dataArray[13] : "Unknown",
                                    Convert.ToInt32(dataArray[14])
                                );
                                birds.Add(nonFlyingBird);
                                break;

                            default:
                                Console.WriteLine($"Unknown type '{type}' found in data. Skipping line: {line}");
                                break;
                        }
                    }
                }

                Console.WriteLine($"Data loaded successfully. {animals.Count} animals and {birds.Count} birds loaded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }

            // Link cages with animals and birds
            LinkCagesWithEntities(animals, birds, cages);
        }

        static void LoadKeepersFromFile(List<HeadKeeper> keepers, List<Cage> cages)
        {
            string filePath = @"..\..\..\keepers.txt"; // ✅ Ensure correct path

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at: {Path.GetFullPath(filePath)}. Starting with an empty list.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine($"🔍 Reading line: {line}"); // ✅ Debugging step

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] dataArray = line.Split(',');

                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            dataArray[i] = dataArray[i].Trim();
                        }

                        string type = dataArray[0].ToLower();

                        if (type == "headkeeper")
                        {
                            var keeper = new HeadKeeper(
                                Convert.ToInt32(dataArray[1]), // Keeper ID
                                dataArray[2], // First Name
                                dataArray[3], // Last Name
                                dataArray[4]  // Position
                            );

                            Console.WriteLine($"✅ DEBUG: Keeper Added -> ID: {keeper.KeeperId}, Name: {keeper.FirstName} {keeper.LastName}");

                            // ✅ Load Assigned Cages
                            if (dataArray.Length > 5 && !string.IsNullOrWhiteSpace(dataArray[5]) && dataArray[5] != "N/A")
                            {
                                string[] cageIds = dataArray[5].Split('|');
                                foreach (var cageId in cageIds)
                                {
                                    if (int.TryParse(cageId, out int parsedCageId))
                                    {
                                        keeper.AssignCage(parsedCageId);
                                    }
                                }
                            }


                            keepers.Add(keeper);
                        }
                        else
                        {
                            Console.WriteLine($"Unknown type '{type}' found in data. Skipping line: {line}");
                        }
                    }
                }

                Console.WriteLine($"✅ Keepers loaded successfully. {keepers.Count} keepers found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading keeper data: {ex.Message}");
            }

            // ✅ Link cages with keepers
            foreach (var keeper in keepers)
            {
                foreach (var cageId in keeper.AssignedCages)
                {
                    var cage = cages.FirstOrDefault(c => c.CageNumber == cageId);
                    if (cage != null)
                    {
                        cage.AssignKeeper(keeper);
                    }
                }
            }
        }


        static void LinkCagesWithEntities(List<Animal> animals, List<Bird> birds, List<Cage> cages)
        {
            foreach (var cage in cages)
            {
                cage.AssignedAnimals.Clear(); // Reset assigned animals
                cage.AssignedBirds.Clear();  // Reset assigned birds
            }

            foreach (var animal in animals)
            {
                var cage = cages.FirstOrDefault(c => c.CageNumber == animal.CageNumber);
                if (cage != null)
                {
                    Console.WriteLine($"DEBUG: Adding {animal.Name} ({animal.Type}) to Cage {animal.CageNumber}");
                    cage.AssignAnimal(animal);
                }
            }

            foreach (var bird in birds)
            {
                var cage = cages.FirstOrDefault(c => c.CageNumber == bird.CageNumber);
                if (cage != null)
                {
                    Console.WriteLine($"DEBUG: Adding {bird.Name} ({bird.Type}) to Cage {bird.CageNumber}");
                    cage.AssignBird(bird);
                }
            }
        }




        // Method to save loans to a file
        // ✅ Keep file path inside method for consistency
        static void SaveLoansToFile(List<Loan> loans)
        {
            string loansFilePath = @"..\..\..\loans.txt"; // ✅ Consistent with LoadLoansFromFile

            try
            {
                using (StreamWriter writer = new StreamWriter(loansFilePath))
                {
                    foreach (var loan in loans)
                    {
                        writer.WriteLine($"{loan.LoanId},{loan.EntityId},{loan.IsAnimal},{loan.EstablishmentDetails},{loan.DateOut},{loan.DateBackIn ?? "N/A"}");
                    }
                }
                Console.WriteLine($"✅ Loans successfully saved to: {Path.GetFullPath(loansFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving loans to file: {ex.Message}");
            }
        }





        // Method to load loans from a file (consistent structure)
        static void LoadLoansFromFile(List<Loan> loans)
        {
            string loansFilePath = @"..\..\..\loans.txt"; // ✅ Ensure correct relative path

            if (!File.Exists(loansFilePath))
            {
                Console.WriteLine($"⚠️ Loans file not found at: {Path.GetFullPath(loansFilePath)}. Starting with an empty list.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(loansFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine($"🔍 Reading loan line: {line}"); // ✅ Debugging step

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] dataArray = line.Split(',');

                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            dataArray[i] = dataArray[i].Trim();
                        }

                        // ✅ Instead of checking for "loan", assume any correctly formatted line is a loan
                        if (dataArray.Length >= 6)
                        {
                            Loan loan = new Loan(
                                Convert.ToInt32(dataArray[0]), // LoanId
                                Convert.ToInt32(dataArray[1]), // EntityId
                                Convert.ToBoolean(dataArray[2]), // IsAnimal
                                dataArray[3], // EstablishmentDetails
                                dataArray[4], // DateOut
                                dataArray[5] == "N/A" ? null : dataArray[5] // DateBackIn (nullable)
                            );

                            loans.Add(loan);
                            Console.WriteLine($"✅ Loaded Loan -> ID: {loan.LoanId}, Entity ID: {loan.EntityId}, Establishment: {loan.EstablishmentDetails}");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Invalid loan data format. Skipping line: {line}");
                        }
                    }
                }

                Console.WriteLine($"✅ Loans loaded successfully. {loans.Count} loans found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading loan data: {ex.Message}");
            }
        }




        
        // Loan out an animal or bird (Fully Functional)
        static void LoanOutEntity(List<Animal> animals, List<Bird> birds, List<Loan> loans)
        {
            string loansFilePath = @"..\..\..\loans.txt"; // ✅ Ensure correct relative path

            Console.WriteLine("Choose the type of entity to loan out:");
            Console.WriteLine("1. Animal");
            Console.WriteLine("2. Bird");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1") // Loan an Animal
            {
                if (animals.Count == 0)
                {
                    Console.WriteLine("No animals available to loan out.");
                    return;
                }

                Console.WriteLine("Available Animals:");
                foreach (var animal in animals)
                    Console.WriteLine($"ID: {animal.AnimalId}, Name: {animal.Name}, Type: {animal.Type}");

                Console.Write("Enter the Animal ID to loan out: ");
                if (!int.TryParse(Console.ReadLine(), out int animalId))
                {
                    Console.WriteLine("Invalid Animal ID.");
                    return;
                }

                Animal selectedAnimal = animals.Find(a => a.AnimalId == animalId);
                if (selectedAnimal == null)
                {
                    Console.WriteLine("Animal not found.");
                    return;
                }

                Console.Write("Enter the establishment details: ");
                string establishment = Console.ReadLine();

                // ✅ Add the loan entry and remove the animal
                loans.Add(new Loan(loans.Count + 1, animalId, true, establishment, DateTime.Now.ToString("yyyy-MM-dd")));
                animals.Remove(selectedAnimal);

                Console.WriteLine($"✅ Animal '{selectedAnimal.Name}' loaned out successfully.");

            }
            else if (choice == "2") // Loan a Bird
            {
                if (birds.Count == 0)
                {
                    Console.WriteLine("No birds available to loan out.");
                    return;
                }

                Console.WriteLine("Available Birds:");
                foreach (var bird in birds)
                    Console.WriteLine($"ID: {bird.BirdId}, Name: {bird.Name}, Type: {bird.Type}");

                Console.Write("Enter the Bird ID to loan out: ");
                if (!int.TryParse(Console.ReadLine(), out int birdId))
                {
                    Console.WriteLine("Invalid Bird ID.");
                    return;
                }

                Bird selectedBird = birds.Find(b => b.BirdId == birdId);
                if (selectedBird == null)
                {
                    Console.WriteLine("Bird not found.");
                    return;
                }

                Console.Write("Enter the establishment details: ");
                string establishment = Console.ReadLine();

                // ✅ Add the loan entry and remove the bird
                loans.Add(new Loan(loans.Count + 1, birdId, false, establishment, DateTime.Now.ToString("yyyy-MM-dd")));
                birds.Remove(selectedBird);

                Console.WriteLine($"✅ Bird '{selectedBird.Name}' loaned out successfully.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            // ✅ Save the updated loans and animals/birds to file
            SaveLoansToFile(loans);
            SaveAnimalsToFile(animals, birds);
        }



        // Method to save borrowed animals/birds to borrowed.txt
        static void SaveBorrowedToFile(List<Borrowed> borrowed)
        {
            string borrowedFilePath = @"..\..\..\borrowed.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(borrowedFilePath))
                {
                    foreach (var item in borrowed)
                    {
                        writer.WriteLine($"borrowed,{item.LoanId},{item.EntityId},{item.IsAnimal},{item.BorrowedFrom},{item.DateOut},{item.DateBackIn ?? "N/A"}");
                    }
                }
                Console.WriteLine($"✅ Borrowed entities successfully saved to: {Path.GetFullPath(borrowedFilePath)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving borrowed data: {ex.Message}");
            }
        }



        // Method to load borrowed animals/birds from borrowed.txt
        static void LoadBorrowedFromFile(List<Borrowed> borrowed)
        {
            string borrowedFilePath = @"..\..\..\borrowed.txt"; // ✅ Ensure correct relative path

            if (!File.Exists(borrowedFilePath))
            {
                Console.WriteLine($"⚠️ Borrowed file not found at: {Path.GetFullPath(borrowedFilePath)}. Starting with an empty list.");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(borrowedFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        Console.WriteLine($"🔍 Reading borrowed line: {line}"); // ✅ Debugging step

                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] dataArray = line.Split(',');

                        for (int i = 0; i < dataArray.Length; i++)
                        {
                            dataArray[i] = dataArray[i].Trim();
                        }

                        if (dataArray[0].ToLower() == "borrowed") // ✅ Ensure correct type
                        {
                            Borrowed borrowedEntity = new Borrowed(
                                Convert.ToInt32(dataArray[1]), // LoanId
                                Convert.ToInt32(dataArray[2]), // EntityId
                                Convert.ToBoolean(dataArray[3]), // IsAnimal
                                dataArray[4], // EstablishmentDetails
                                dataArray[5], // DateOut
                                dataArray[6] == "N/A" ? null : dataArray[6] // DateBackIn (nullable)
                            );

                            borrowed.Add(borrowedEntity);
                            Console.WriteLine($"✅ DEBUG: Borrowed {borrowedEntity.LoanId} loaded - Entity ID {borrowedEntity.EntityId}");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ Unknown type '{dataArray[0]}' found in borrowed.txt. Skipping line: {line}");
                        }
                    }
                }

                Console.WriteLine($"✅ Borrowed entities loaded successfully. {borrowed.Count} entries found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error loading borrowed data: {ex.Message}");
            }
        }


        // Borrow an Animal or Bird
        static void BorrowEntity(List<Animal> animals, List<Bird> birds, List<Borrowed> borrowed)
        {
            Console.WriteLine("Choose the type of entity to borrow:");
            Console.WriteLine("1. Animal");
            Console.WriteLine("2. Bird");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Enter the Animal ID to borrow: ");
                int animalId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter the name of the establishment you are borrowing from: ");
                string borrowedFrom = Console.ReadLine();

                borrowed.Add(new Borrowed(borrowed.Count + 1, animalId, true, borrowedFrom, DateTime.Now.ToString("yyyy-MM-dd")));

                Console.WriteLine($"Animal ID {animalId} successfully borrowed from {borrowedFrom}.");
            }
            else if (choice == "2")
            {
                Console.Write("Enter the Bird ID to borrow: ");
                int birdId = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter the name of the establishment you are borrowing from: ");
                string borrowedFrom = Console.ReadLine();

                borrowed.Add(new Borrowed(borrowed.Count + 1, birdId, false, borrowedFrom, DateTime.Now.ToString("yyyy-MM-dd")));

                Console.WriteLine($"Bird ID {birdId} successfully borrowed from {borrowedFrom}.");
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }





    }
}














