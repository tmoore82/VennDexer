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
        /// Checks to make sure the directories specified by the user exist. 
        /// If they don't, it either creates them or throws an exception 
        /// depending on whether or not the user has given us permission to 
        /// create directories. 
        /// 
        /// Will not overwrite existing directories.
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        internal static Config validate(Config cfg)
        {
            if (cfg.zipDir != "" & !Directory.Exists(cfg.zipDir))
            {
                throw new DirectoryNotFoundException(cfg.zipDir);
            }
            else if (cfg.zipDir != "" & !Directory.Exists(cfg.extractDir) & cfg.createDirs == false)
            {
                throw new DirectoryNotFoundException(cfg.extractDir);
            }
            else if (cfg.zipDir != "" & !Directory.Exists(cfg.extractDir) & cfg.createDirs == true)
            {
                Directory.CreateDirectory(cfg.extractDir);
            }

            if (!Directory.Exists(cfg.resultsDir) & cfg.createDirs == true)
            {
                Directory.CreateDirectory(cfg.resultsDir);
            }
            else
            { 
                throw new DirectoryNotFoundException(cfg.resultsDir);
            }

            return cfg;
        }
    }
}
