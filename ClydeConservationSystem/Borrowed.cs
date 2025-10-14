using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    // Borrowed class inherits from Loan
    class Borrowed : Loan
    {
        public string BorrowedFrom { get; set; } // Establishment from where it was borrowed

        // Constructor
        public Borrowed(int loanId, int entityId, bool isAnimal, string borrowedFrom, string dateBorrowed, string? dateReturned = null)
            : base(loanId, entityId, isAnimal, borrowedFrom, dateBorrowed, dateReturned)
        {
            BorrowedFrom = borrowedFrom;
        }

        // Method to record the return of the borrowed entity
        public override string ToString()
        {
            return $"Borrowed ID: {LoanId}, Entity ID: {EntityId}, Type: {(IsAnimal ? "Animal" : "Bird")}, Borrowed From: {BorrowedFrom}, Date Borrowed: {DateOut}, Date Returned: {DateBackIn ?? "Not Yet Returned"}";
        }
    }

}
