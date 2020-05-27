using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class ResultLogic : BaseLogic
    {
        public void AddResult(string fileName, string filePath, int searchId)
        {
            Result result = new Result() { Filename = fileName, Path = filePath, SearchID = searchId };
            db.Results.Add(result);
            db.SaveChanges();
        }
    }
}
