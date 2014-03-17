using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VennDexer
{
    public static partial class Venngine
    {
        /// <summary>
        /// The heavy lifting takes place here. 
        /// The file lists are created and analyzed.
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        internal static FileStat ignite(Config cfg)
        {
            FileStat stat = new FileStat();

            stat.fileSet = new List<string>();
            stat.fileNoIndex = new List<string>();
            stat.indexNoFile = new List<string>();
            stat.totalMatches = new List<string>();
            stat.duplicates = new List<string>();

            stat.indexFile = cfg.index.file;
            stat.resultsDir = cfg.resultsDir;

            if (cfg.zipDir != "")
            {
                extract(cfg.zipDir, cfg.extractDir);
                cfg.srcDirs = recurSrc(cfg.extractDir.Split(cfg.index.delimiter).ToArray(), cfg.srcDirs);
            }
            
            stat = recurDirs(cfg.srcDirs, stat);

            List<string> indexLines = new List<string>();

            using (StreamReader sr = new StreamReader(cfg.index.file))
            {
                while (sr.Peek() != -1)
                {
                    indexLines.Add(sr.ReadLine());
                }
            }

            stat.indexRecordCount = indexLines.Count;

            List<string> matches = new List<string>();

            foreach (string file in indexLines)
            {
                string fileName = file.Split(cfg.index.delimiter).ToArray()[cfg.index.fileNameColumn];
                if (stat.fileSet.Contains(fileName))
                {
                    stat.totalMatches.Add(file);
                    matches.Add(fileName);
                }
                else
                {
                    stat.indexNoFile.Add(file);
                }
            }

            stat.fileNoIndex = stat.fileSet.Except(matches).ToList<string>();

            return stat;
        }

        /// <summary>
        /// Iterates through the source directories collected in getSrc 
        /// and adds all non-duplicate files to our file set. 
        /// Duplicates are added to a separate list.
        /// </summary>
        /// <param name="srcDirs"></param>
        /// <param name="stat"></param>
        /// <returns></returns>
        private static FileStat recurDirs(List<string> srcDirs, FileStat stat)
        {
            string fileName;

            foreach (string dir in srcDirs)
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    fileName = Path.GetFileName(file);
                    if (!stat.fileSet.Contains(fileName))
                    {
                        stat.fileSet.Add(fileName);
                    }
                    else
                    {
                        stat.duplicates.Add(file);
                    }
                }
            }

            return stat;
        }
    }
}
