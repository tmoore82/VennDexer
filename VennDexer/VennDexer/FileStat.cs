using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VennDexer
{
    /// <summary>
    /// Object for storing and retrieving the stats
    /// we're interested in.
    /// </summary>
    public class FileStat
    {
        public string indexFile { get; set; }
        public string resultsDir { get; set; }
        public int indexRecordCount { get; set; }
        public List<string> fileSet { get; set; }
        public List<string> duplicates { get; set; }
        public List<string> totalMatches { get; set; }
        public List<string> indexNoFile { get; set; }
        public List<string> fileNoIndex { get; set; }
    }
}
