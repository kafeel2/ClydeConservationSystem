using ClydeConservationSystem;

public class Cage
{
    private const int MaxCageNumber = 8; // Maximum allowable cage number

    public int CageNumber { get; private set; }
    public string Description { get; private set; }
    public int MaxAnimals { get; private set; }
    public List<Animal> AssignedAnimals { get; private set; }
    public List<Bird> AssignedBirds { get; private set; }
    public HeadKeeper? ResponsibleKeeper { get; private set; }
    public bool IsActive { get; private set; } // New flag

    public Cage(int cageNumber, string description, int maxAnimals)
    {
        if (cageNumber < 1 || cageNumber > MaxCageNumber)
        {
            throw new ArgumentOutOfRangeException(nameof(cageNumber), $"Cage number must be between 1 and {MaxCageNumber}.");
        }

        CageNumber = cageNumber;
        Description = description;
        MaxAnimals = maxAnimals;
        AssignedAnimals = new List<Animal>();
        AssignedBirds = new List<Bird>();
        IsActive = true; // Active by default
    }

    private bool IsCompatible(Animal newAnimal, Animal existingAnimal)
    {
        // Debugging: Print the comparison details
        //Console.WriteLine($"DEBUG: Comparing '{newAnimal.Type.ToLower()}' (Danger: {newAnimal.DangerRating}) with '{existingAnimal.Type.ToLower()}' (Danger: {existingAnimal.DangerRating})");

        // Check danger rating compatibility
        if (newAnimal.DangerRating != existingAnimal.DangerRating)
        {
            Console.WriteLine($"Incompatible: Danger ratings do not match.");
            return false;
        }

        // Sharing rules dictionary with case-insensitivity
        var sharingRules = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "ape", new List<string> { "ape" } },
        { "marmoset monkey", new List<string> { "zebra", "marmoset monkey" } },
        { "tiger", new List<string> { "tiger" } },
        { "rabbit", new List<string> { "guinea pig", "rabbit" } },
        { "guinea pig", new List<string> { "rabbit", "guinea pig" } },
        { "horse", new List<string> { "donkey", "horse" } },
        { "donkey", new List<string> { "horse", "donkey" } },
        { "chameleon", new List<string> { "chameleon" } },
        { "bearded dragon", new List<string> { "lizard", "bearded dragon" } },
        { "lizard", new List<string> { "bearded dragon", "lizard" } },
        { "owl", new List<string> { "owl" } },
        { "vulture", new List<string> { "vulture" } },
        { "emu", new List<string> { "emu" } },
        { "penguin", new List<string> { "penguin" } },
        { "zebra", new List<string> { "marmoset monkey", "zebra" } },
        { "eagle", new List<string> { "eagle" } },
        { "snake", new List<string> { "snake" } },
        { "crocodile", new List<string> { "crocodile" } }
    };

        // Check species compatibility
        if (!sharingRules.TryGetValue(newAnimal.Type.ToLower(), out var compatibleTypes) ||
            !compatibleTypes.Contains(existingAnimal.Type.ToLower()))
        {
            Console.WriteLine($"Incompatible: {newAnimal.Type} cannot share a cage with {existingAnimal.Type}.");
            return false;
        }

        Console.WriteLine($"Compatible: {newAnimal.Type} can share a cage with {existingAnimal.Type}.");
        return true;
    }


    public bool CheckSuitability(Animal animal)
    {
        // Debug: Print all currently assigned animals in the cage
        Console.WriteLine($"DEBUG: Verifying suitability for {animal.Name} ({animal.Type}) in Cage {CageNumber}.");
        Console.WriteLine($"DEBUG: Current Assigned Animals in Cage {CageNumber}:");
        foreach (var assignedAnimal in AssignedAnimals)
        {
            Console.WriteLine($"  - Name: {assignedAnimal.Name}, Type: {assignedAnimal.Type}, Danger Rating: {assignedAnimal.DangerRating}");
        }

        // Check if cage is already at capacity
        if (AssignedAnimals.Count >= MaxAnimals)
        {
            Console.WriteLine($"Cage {CageNumber} is full. It cannot accommodate more animals.");
            return false;
        }

        // Check compatibility of the new animal with all assigned animals
        foreach (var assignedAnimal in AssignedAnimals)
        {
            Console.WriteLine($"Checking compatibility between {animal.Name} ({animal.Type}) and {assignedAnimal.Name} ({assignedAnimal.Type}) in Cage {CageNumber}.");

            if (!IsCompatible(animal, assignedAnimal))
            {
                Console.WriteLine($"Incompatibility found: {animal.Type} cannot share a cage with {assignedAnimal.Type}.");
                return false;
            }
        }

        Console.WriteLine($"Cage {CageNumber} is suitable for {animal.Name} ({animal.Type}).");
        return true;
    }

    private bool IsCompatible(Bird newBird, Bird existingBird)
    {
        // Debugging: Print the comparison details
        Console.WriteLine($"DEBUG: Comparing '{newBird.Type.ToLower()}' (Danger: {newBird.DangerRating}) with '{existingBird.Type.ToLower()}' (Danger: {existingBird.DangerRating})");

        // Check danger rating compatibility
        if (newBird.DangerRating != existingBird.DangerRating)
        {
            Console.WriteLine($"Incompatible: Danger ratings do not match.");
            return false;
        }

        // Sharing rules dictionary with case-insensitivity
        var birdSharingRules = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
    {
        { "owl", new List<string> { "owl" } },
        { "vulture", new List<string> { "vulture" } },
        { "emu", new List<string> { "emu" } },
        { "penguin", new List<string> { "penguin" } },
        { "eagle", new List<string> { "eagle" } }
        // Add more bird-specific sharing rules here if needed
    };

        // Check species compatibility
        if (!birdSharingRules.TryGetValue(newBird.Type.ToLower(), out var compatibleTypes) ||
            !compatibleTypes.Contains(existingBird.Type.ToLower()))
        {
            Console.WriteLine($"Incompatible: {newBird.Type} cannot share a cage with {existingBird.Type}.");
            return false;
        }

        Console.WriteLine($"Compatible: {newBird.Type} can share a cage with {existingBird.Type}.");
        return true;
    }



    public bool CheckSuitability(Bird bird)
    {
        // Debug: Print all currently assigned birds in the cage
        Console.WriteLine($"DEBUG: Verifying suitability for {bird.Name} ({bird.Type}) in Cage {CageNumber}.");
        Console.WriteLine($"DEBUG: Current Assigned Birds in Cage {CageNumber}:");
        foreach (var assignedBird in AssignedBirds)
        {
            Console.WriteLine($"  - Name: {assignedBird.Name}, Type: {assignedBird.Type}, Danger Rating: {assignedBird.DangerRating}");
        }

        // Check if cage is already at capacity
        if (AssignedBirds.Count >= MaxAnimals)
        {
            Console.WriteLine($"Cage {CageNumber} is full. It cannot accommodate more birds.");
            return false;
        }

        // Check compatibility of the new bird with all assigned birds
        foreach (var assignedBird in AssignedBirds)
        {
            Console.WriteLine($"Checking compatibility between {bird.Name} ({bird.Type}) and {assignedBird.Name} ({assignedBird.Type}) in Cage {CageNumber}.");

            if (!IsCompatible(bird, assignedBird))
            {
                Console.WriteLine($"Incompatibility found: {bird.Type} cannot share a cage with {assignedBird.Type}.");
                return false;
            }
        }

        Console.WriteLine($"Cage {CageNumber} is suitable for {bird.Name} ({bird.Type}).");
        return true;
    }


    public bool AssignAnimal(Animal animal)
    {
        if (CheckSuitability(animal))
        {
            AssignedAnimals.Add(animal);
            SaveCageDetailsToFile(); // Save cage details if needed
            return true;
        }

        Console.WriteLine($"Cage {CageNumber} is not suitable for {animal.Name} ({animal.Type}).");
        return false;
    }


    public bool AssignBird(Bird bird)
    {
        if (CheckSuitability(bird))
        {
            AssignedBirds.Add(bird);
            SaveCageDetailsToFile();
            return true;
        }
        return false;
    }

    public void SetInactive()
    {
        IsActive = false;
        Console.WriteLine($"Cage {CageNumber} is now inactive.");
    }


    public void ShowCageDetails()
    {
        Console.WriteLine($"Cage Number: {CageNumber}, Description: {Description}, Max Capacity: {MaxAnimals}");
        Console.WriteLine(IsActive ? "Status: Active" : "Status: Inactive");
        Console.WriteLine("Assigned Animals:");
        foreach (var animal in AssignedAnimals)
        {
            Console.WriteLine($"- {animal.Name} ({animal.Type})");
        }
        Console.WriteLine("Assigned Birds:");
        foreach (var bird in AssignedBirds)
        {
            Console.WriteLine($"- {bird.Name} ({bird.Type})");
        }
        Console.WriteLine($"Responsible Keeper: {(ResponsibleKeeper != null ? ResponsibleKeeper.FirstName + " " + ResponsibleKeeper.LastName : "None")}");
    }



    public bool AssignKeeper(HeadKeeper keeper)
    {
        if (ResponsibleKeeper != null)
        {
            Console.WriteLine($"Cage {CageNumber} already has a keeper assigned.");
            return false;
        }

        if (keeper.AssignedCages.Count >= 4)
        {
            Console.WriteLine($"Keeper {keeper.FirstName} cannot manage more than 4 cages.");
            return false;
        }

        ResponsibleKeeper = keeper;
        keeper.AssignCage(CageNumber);

        return true;
    }

    public void RemoveKeeper()
    {
        if (ResponsibleKeeper != null)
        {
            Console.WriteLine($"Keeper {ResponsibleKeeper.FirstName} removed from Cage {CageNumber}.");
            ResponsibleKeeper = null; // Remove the keeper reference
        }
    }



    public void SaveCageDetailsToFile()
    {
        string filePath = @"..\..\..\cages.txt"; // Ensure correct path

        try
        {
            List<string> lines = new List<string>();

            if (File.Exists(filePath))
            {
                lines = File.ReadAllLines(filePath).ToList();

                // Remove old cage entry if it exists
                lines.RemoveAll(line => line.StartsWith($"{CageNumber},"));
            }

            // Add the updated cage details
            lines.Add($"{CageNumber},{Description},{MaxAnimals},{(IsActive ? "Active" : "Inactive")}");

            File.WriteAllLines(filePath, lines);

            Console.WriteLine($"✅ Cage {CageNumber} details saved successfully to {filePath}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error saving cage details: {ex.Message}");
        }
    }


    public static void SaveCagesToFile(List<Cage> cages)
    {
        string cagesFilePath = @"..\..\..\cages.txt";

        try
        {
            using (StreamWriter writer = new StreamWriter(cagesFilePath, false)) // ✅ Overwrites file to prevent duplicates
            {
                foreach (var cage in cages)
                {
                    writer.WriteLine($"{cage.CageNumber},{cage.Description},{cage.MaxAnimals},{(cage.IsActive ? "Active" : "Inactive")}");
                }
            }
            Console.WriteLine("✅ Cage data successfully saved to cages.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error saving cage data: {ex.Message}");
        }
    }


    public void RemoveCage()
    {
        Console.WriteLine($"Cage {CageNumber} is now unassigned and removed from the system.");
        IsActive = false; // ✅ Mark cage as inactive
        SaveCagesToFile(new List<Cage> { this }); // ✅ Save only this cage update
    }



    internal void PrintDetails()
    {
        throw new NotImplementedException();
    }
}



