using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VennDexer
{
    public static partial class Venngine
    {
        /// <summary>
        /// Main entry point for the library.
        /// </summary>
        /// <param name="configLoc"></param>
        /// <returns></returns>
        public static List<FileStat> crank(string configLoc)
        {
            XmlDocument config = new XmlDocument();

            config.Load(configLoc);

            List<FileStat> stats = new List<FileStat>();

            List<Config> cfgs = parseConfig(config);

            foreach (Config cfg in cfgs)
            {
                validate(cfg);
                stats.Add(ignite(cfg));
            }

            return stats;
        }
    }
}
