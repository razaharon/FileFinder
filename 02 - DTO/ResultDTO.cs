using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class ResultDTO
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string File { get; set; }
        public int SearchID { get; set; }
    }
}
