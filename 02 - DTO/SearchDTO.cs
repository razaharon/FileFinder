using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class SearchDTO
    {
        public int ID { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public int? Results { get; set; }
    }
}
