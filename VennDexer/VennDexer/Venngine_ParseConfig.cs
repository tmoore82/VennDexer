using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VennDexer
{
    public static partial class Venngine
    {
        /// <summary>
        /// Parses the XML document provided by the user 
        /// to assign our Config properties.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        internal static List<Config> parseConfig(XmlDocument config)
        {
            List<Config> cfgs = new List<Config>();
            XmlNodeList nodes = config.DocumentElement.SelectNodes("fileSet");

            foreach (XmlNode node in nodes)
            {
                Config cfg = new Config();
                cfg.srcDirs = new List<string>();

                XmlNodeList srcDirs = node.SelectNodes("srcDirs/src");
                List<string> tmpSrcDirs = new List<string>();

                for (int i = 0; i < srcDirs.Count; i++)
                {
                    tmpSrcDirs.Add(srcDirs[i].InnerText);
                }

                if ((cfg.areZipped = node.SelectSingleNode("srcDirs/areZipped").InnerText == "yes" ? true : false))
                {
                    if ((cfg.doExtract = node.SelectSingleNode("srcDirs/doExtract").InnerText == "yes" ? true : false))
                    {
                        cfg.extractDir = node.SelectSingleNode("srcDirs/extractDir").InnerText;
                    }
                }

                cfg.srcDirs = recurSrc(tmpSrcDirs);

                cfg.resultsDir = node.SelectSingleNode("resultsDir").InnerText;

                cfg.index = new Config.IndexFile();
                cfg.index.file = node.SelectSingleNode("index/file").InnerText;

                if (node.SelectSingleNode("index/delimited").InnerText != "" & node.SelectSingleNode("index/delimited").InnerText == "yes")
                {
                    cfg.index.delimited = true;
                    string delimeter = node.SelectSingleNode("index/delimiter").InnerText;
                    char delChar;

                    switch (delimeter)
                    {
                        case "t":
                            delChar = Convert.ToChar(9);
                            break;
                        case "s":
                            delChar = Convert.ToChar(32);
                            break;
                        default:
                            delChar = Convert.ToChar(delimeter);
                            break;
                    }

                    cfg.index.delimiter = delChar;
                    cfg.index.fileNameColumn = Convert.ToInt32(node.SelectSingleNode("index/fileNameColumn").InnerText);
                }
                else
                {
                    cfg.index.delimited = false;
                    cfg.index.delimiter = Convert.ToChar('\u0000');
                    cfg.index.fileNameColumn = 0;
                }

                cfgs.Add(cfg);
            }

            return cfgs;
        }

        /// <summary>
        /// Recursive function that goes through all subdirectories 
        /// in a provided array and adds them to a provided list of sources.
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static List<string> recurSrc(List<string> srcDirs)
        {
            foreach (string src in srcDirs)
            {
                if (Directory.GetDirectories(src).Length > 0)
                {
                    srcDirs.AddRange(recurSrc(Directory.GetDirectories(src).ToList<string>()));
                }

                if (Directory.Exists(src) & !srcDirs.Contains(src))
                {
                    srcDirs.Add(src);
                }
                else if(!Directory.Exists(src))
                {
                    throw new DirectoryNotFoundException(src);
                }
            }

            return srcDirs;
        }
    }
}
