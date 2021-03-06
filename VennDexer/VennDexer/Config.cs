﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VennDexer
{
    /// <summary>
    /// Object for storing and retrieving configuration
    /// settings imported from the user's XML document
    /// </summary>
    internal class Config
    {
        internal string resultsDir { get; set; }
        internal bool areZipped { get; set; }
        internal bool doExtract { get; set; }
        internal string extractDir { get; set; }
        internal List<string> srcDirs { get; set; }
        internal IndexFile index { get; set; }

        /// <summary>
        /// Nested object for settings related to the CSV index
        /// </summary>
        internal class IndexFile
        {
            internal string file { get; set; }
            internal bool delimited { get; set; }
            internal char delimiter { get; set; }
            internal int fileNameColumn { get; set; }
        }
    }
}
