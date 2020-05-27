using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class BaseLogic : IDisposable
    {
        protected FileFinderEntities db = new FileFinderEntities();

        public void ResetDB()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Results]");
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Searches]");
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
