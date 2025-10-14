using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClydeConservationSystem
{
    public class WeeklyReport
    {
        public int ReportId { get; private set; }
        public string VetVisitDetails { get; private set; }
        public List<Cage> CageReports { get; private set; }
        public List<HeadKeeper> KeeperReports { get; private set; }

        public WeeklyReport(int reportId)
        {
            ReportId = reportId;
            VetVisitDetails = string.Empty;
            CageReports = new List<Cage>();
            KeeperReports = new List<HeadKeeper>();
        }

        public void IncludeVetVisitDetails(string details)
        {
            VetVisitDetails = details;
            Console.WriteLine("Vet visit details added to the report.");
        }

        public void GenerateReport()
        {
            Console.WriteLine($"Report ID: {ReportId}");
            Console.WriteLine($"Vet Visit Details: {VetVisitDetails}");
            Console.WriteLine("Cage Reports:");
            foreach (var cage in CageReports)
            {
                Console.WriteLine($"Cage {cage.CageNumber}: {cage.Description}");
            }
            Console.WriteLine("Keeper Reports:");
            foreach (var keeper in KeeperReports)
            {
                Console.WriteLine($"Keeper {keeper.KeeperId}: {keeper.FirstName} {keeper.LastName}");
            }
        }
    }

}
