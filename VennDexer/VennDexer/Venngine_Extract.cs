using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VennDexer
{
    public static partial class Venngine
    {
        /// <summary>
        /// Extracts all files from compressed files 
        /// specified by the user.
        /// 
        /// Checks the signature to verify it's a zip file 
        /// and of a type that we can process.
        /// </summary>
        /// <param name="zipDir"></param>
        /// <param name="extractDir"></param>
        internal static void extract(string zipDir, string extractDir)
        {
            Dictionary<string, int[,]> zipSigs = new Dictionary<string, int[,]>();
            //zipSigs.Add("1F-8B-08", new int[1, 2] { { 3, 0 } });
            //zipSigs.Add("37-7A-BC-AF-27-1C", new int[1, 2] { { 6, 0 } });
            zipSigs.Add("50-4B-03-04", new int[1, 2] { { 4, 0 } });
            zipSigs.Add("50-4B-05-06", new int[1, 2] { { 4, 0 } });
            zipSigs.Add("50-4B-05-08", new int[1, 2] { { 4, 0 } });
            //zipSigs.Add("57-69-6E-5A-69-70", new int[1, 2] { { 6, 289152 } });

            foreach (string file in Directory.GetFiles(zipDir))
            {
                foreach (KeyValuePair<string, int[,]> entry in zipSigs)
                {
                    if (SigChecker.SigChecker.CheckSignature(file, entry.Value[0, 0], entry.Value[0, 1], entry.Key))
                    {
                        string newDir = extractDir + "\\" + Path.GetFileNameWithoutExtension(file);
                        Directory.CreateDirectory(newDir);
                        ZipFile.ExtractToDirectory(file, newDir);
                        break;
                    }
                }
            }
        }
    }
}
