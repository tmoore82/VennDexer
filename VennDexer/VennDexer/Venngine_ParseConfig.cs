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
                cfg.index = new Config.IndexFile();
                cfg.createDirs = node.SelectSingleNode("createDirs").InnerText == "yes" ? true : false;
                cfg.zipDir = node.SelectSingleNode("zipDir").InnerText;
                cfg.extractDir = node.SelectSingleNode("extractDir").InnerText;
                cfg.srcDirs = getSrc(node.SelectSingleNode("srcDirs/src").InnerText);
                cfg.resultsDir = node.SelectSingleNode("resultsDir").InnerText;

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
        /// Goes through the source directories specified by the user 
        /// and adds them to our list of source directories.
        /// </summary>
        /// <param name="usrSrc"></param>
        /// <returns></returns>
        private static List<string> getSrc(string usrSrc)
        {
            List<string> sources = new List<string>();

            if (usrSrc == "zipDir")
            {
                return sources;
            }
            else
            {
                string[] dirs = usrSrc.Split(',');
 
                return recurSrc(dirs, sources);
            }
        }

        /// <summary>
        /// Recursive function that goes through all subdirectories 
        /// in a provided array and adds them to a provided list of sources.
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        private static List<string> recurSrc(string[] dirs, List<string> sources)
        {
            foreach (string dir in dirs)
            {
                if (Directory.GetDirectories(dir).Length > 0)
                { 
                    recurSrc(Directory.GetDirectories(dir),sources);
                }

                if (Directory.Exists(dir))
                {
                    sources.Add(dir);
                }
                else
                {
                    throw new DirectoryNotFoundException(dir);
                }
            }

            return sources;
        }
    }
}
