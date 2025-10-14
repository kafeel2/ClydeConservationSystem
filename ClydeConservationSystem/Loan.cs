using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    // Class to represent a loan of an animal
    public class Loan
    {
        public int LoanId { get; private set; }
        public int EntityId { get; private set; } // ID of the animal or bird
        public bool IsAnimal { get; private set; } // True if Animal, False if Bird
        public string EstablishmentDetails { get; private set; }
        public string DateOut { get; private set; }
        public string? DateBackIn { get; private set; }

        public Loan(int loanId, int entityId, bool isAnimal, string establishmentDetails, string dateOut, string? dateBackIn = null)
        {
            LoanId = loanId;
            EntityId = entityId;
            IsAnimal = isAnimal;
            EstablishmentDetails = establishmentDetails;
            DateOut = dateOut;
            DateBackIn = dateBackIn;
        }

        public void RecordReturn(string returnDate)
        {
            DateBackIn = returnDate;
            Console.WriteLine($"Loan {LoanId} returned on {returnDate}.");
        }
    }


}
