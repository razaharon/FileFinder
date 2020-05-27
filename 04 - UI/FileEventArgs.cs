using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileFinder
{
    public class FileEventArgs : EventArgs
    {
        public string fileName { get; set; }
        public string extension { get; set; }
        public string path { get; set; }

        public FileEventArgs(string filename,string fileextension, string filepath)
        {
            fileName = filename;
            extension = fileextension;
            path = filepath;
        }
    }
}
