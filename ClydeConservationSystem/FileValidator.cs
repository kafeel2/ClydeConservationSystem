/*using System;
using System.IO;

namespace ClydeConservationSystem
{
    public class FileValidator
    {
        public static void ValidateAnimalsFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: animals.txt");
                return;
            }

            string[] lines = File.ReadAllLines(filePath);
            int lineNumber = 1;

            foreach (var line in lines)
            {
                Console.WriteLine($"Validating line {lineNumber}: {line}");
                string[] parts = line.Split('|');

                if (parts.Length < 3)
                {
                    Console.WriteLine($"Error on line {lineNumber}: Insufficient fields. Expected at least 3, found {parts.Length}.");
                    lineNumber++;
                    continue;
                }

                string type = parts[0].Trim();
                if (!int.TryParse(parts[1], out int animalId))
                {
                    Console.WriteLine($"Error on line {lineNumber}: Invalid Animal ID '{parts[1]}'.");
                }

                string name = parts[2].Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine($"Error on line {lineNumber}: Animal name is missing.");
                }

                if (parts.Length >= 4 && !DateTime.TryParse(parts[3], out DateTime dateOfBirth))
                {
                    Console.WriteLine($"Error on line {lineNumber}: Invalid Date of Birth '{parts[3]}'.");
                }

                // Add additional checks for specific types
                switch (type.ToLower())
                {
                    case "mammal":
                        if (parts.Length < 12)
                            Console.WriteLine($"Error on line {lineNumber}: Mammal requires 12 fields, found {parts.Length}.");
                        break;

                    case "reptile":
                        if (parts.Length < 11)
                            Console.WriteLine($"Error on line {lineNumber}: Reptile requires 11 fields, found {parts.Length}.");
                        break;

                    case "flyingbird":
                    case "nonflyingbird":
                        if (parts.Length < 13)
                            Console.WriteLine($"Error on line {lineNumber}: Bird requires 13 fields, found {parts.Length}.");
                        break;

                    default:
                        Console.WriteLine($"Error on line {lineNumber}: Unknown type '{type}'.");
                        break;
                }

                lineNumber++;
            }
            Console.WriteLine("Validation complete.");
        }
    }
}*/

