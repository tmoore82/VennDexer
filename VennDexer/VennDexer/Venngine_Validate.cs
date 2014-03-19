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
            foreach (string dir in cfg.srcDirs)
            {
                if (!Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException(dir);
                }
            }

            if (!Directory.Exists(cfg.resultsDir))
            {
                Directory.CreateDirectory(cfg.resultsDir);
            }
            else
            {
                throw new UnauthorizedAccessException(cfg.resultsDir);
            }

            if (cfg.extractDir != null)
            {
                if (!Directory.Exists(cfg.extractDir))
                {
                    Directory.CreateDirectory(cfg.extractDir);
                }
                else
                {
                    throw new UnauthorizedAccessException(cfg.extractDir);
                }
            }

            return cfg;
        }
    }
}
