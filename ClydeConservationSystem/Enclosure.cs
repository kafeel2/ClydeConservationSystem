using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    public class Enclosure
    {
        public int EnclosureId { get; private set; }
        public string Location { get; private set; }
        public int MaxCages { get; private set; }
        public List<Cage> Cages { get; private set; }

        public Enclosure(int enclosureId, string location, int maxCages)
        {
            EnclosureId = enclosureId;
            Location = location;
            MaxCages = maxCages;
            Cages = new List<Cage>();
        }

        public bool AddCage(Cage cage)
        {
            if (Cages.Count >= MaxCages)
            {
                Console.WriteLine("Cannot add more cages to this enclosure.");
                return false;
            }
            Cages.Add(cage);
            return true;
        }

        public static void RemoveCage(List<Cage> cages, List<Animal> animals, List<Bird> birds)
        {
            Console.Write("Enter the Cage Number to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int cageNumber))
            {
                Console.WriteLine("❌ Invalid Cage Number.");
                return;
            }

            var cageToRemove = cages.FirstOrDefault(c => c.CageNumber == cageNumber);
            if (cageToRemove == null)
            {
                Console.WriteLine($"❌ No cage found with number {cageNumber}.");
                return;
            }

            // ✅ Step 1: Unassign all animals and birds from this cage
            foreach (var animal in animals.Where(a => a.CageNumber == cageNumber).ToList())
            {
                animal.CageNumber = 0; // Mark as unassigned
                Console.WriteLine($"⚠️ Animal '{animal.Name}' is now unassigned from Cage {cageNumber}.");
            }

            foreach (var bird in birds.Where(b => b.CageNumber == cageNumber).ToList())
            {
                bird.CageNumber = 0; // Mark as unassigned
                Console.WriteLine($"⚠️ Bird '{bird.Name}' is now unassigned from Cage {cageNumber}.");
            }

            // ✅ Step 2: Remove the cage from the list
            cages.Remove(cageToRemove);
            Console.WriteLine($"✅ Cage {cageNumber} removed successfully.");

            // ✅ Step 3: Save updates
            Cage.SaveCagesToFile(cages);
            Program.SaveAnimalsToFile(animals, birds); // ✅ Ensure animals are updated
        }





    }

}
