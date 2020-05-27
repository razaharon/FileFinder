using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class SearchLogic : BaseLogic
    {
        public SearchDTO AddSearch(string value, string path)
        {
            Search result = db.Searches.Add(new Search() { SearchValue = value, SearchPath = path });
            db.SaveChanges();
            return new SearchDTO() { ID = result.SearchID, Path = result.SearchPath, Value = result.SearchValue };
        }

        public List<SearchDTO> GetAllSearches()
        {
            return db.Searches
                .Select(s => new SearchDTO() { ID = s.SearchID, Path = s.SearchPath, Value = s.SearchValue, Results=s.Results }).ToList();
        }

        public void SetSearchResults(int searchId, int resultsCount)
        {
            Search result = db.Searches.Where(s => s.SearchID == searchId).FirstOrDefault();
            result.Results = resultsCount;
            db.SaveChanges();
        }
    }
}
