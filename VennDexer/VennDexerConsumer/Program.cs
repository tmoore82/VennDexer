using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VennDexer;

namespace VennDexerConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("VennDexer v2.0 | tmoore82 | 2014.03.18");

            Console.WriteLine();
            Console.Write("Press Enter to begin...");
            Console.ReadLine();

            DateTime start = DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("Start time: " + start.ToString());

            //string configLoc = args[0];
            string configLoc = @"C:\users\tmoore\Desktop\eaglelift-venndexer-config.xml";

            List<VennDexer.FileStat> results = new List<VennDexer.FileStat>();

            Console.WriteLine();
            Console.Write("Performing analysis...");
            try
            {
                results = Venngine.crank(configLoc);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine();
                Console.WriteLine("Error: " + e.Message + " not found. Please correct the error and try again.");
                Console.WriteLine();
                Console.Write("Program will exit when you hit enter...");
                Console.ReadLine();

                Environment.Exit(0);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message + ". Please correct the error and try again.");
                Console.WriteLine();
                Console.Write("Program will exit when you hit enter...");
                Console.ReadLine();

                Environment.Exit(0);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.Write("Program will exit when you hit enter...");
                Console.ReadLine();

                Environment.Exit(0);
            }
            catch (IOException e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                Console.WriteLine();
                Console.Write("Program will exit when you hit enter...");
                Console.ReadLine();

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine("An unknown error occured.");
                Console.WriteLine("Error message: " + e.Message);
                Console.WriteLine("InnerException: " + e.InnerException);
                Console.WriteLine();
                Console.WriteLine("Please try to correct the error and try again.");
                Console.WriteLine();
                Console.Write("Program will exit when you hit enter...");
                Console.ReadLine();

                Environment.Exit(0);
            }

            DateTime finish = DateTime.Now;
            TimeSpan elapsed = finish - start;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Finish time: " + finish.ToString());
            Console.WriteLine("Total running time: " + elapsed.ToString());


            foreach (FileStat stat in results)
            {

                string resultsMsg = "    RESULTS FOR " + stat.indexFile + Environment.NewLine
                                    + "    --------------------------------------------------------" + Environment.NewLine
                                    + "    Number of index records: " + stat.indexRecordCount + Environment.NewLine
                                    + "    Number of files found: " + stat.fileSet.Count + Environment.NewLine
                                    + "    Number of duplicate files (not included in total): " + stat.duplicates.Count + Environment.NewLine
                                    + "    Number of file-to-index matches: " + stat.totalMatches.Count + Environment.NewLine
                                    + "    Number of index records with no matching file: " + stat.indexNoFile.Count + Environment.NewLine
                                    + "    Number of files with no index record: " + stat.fileNoIndex.Count;

                Console.WriteLine();
                Console.WriteLine(resultsMsg);

                writeResults("    Write these results to file? (y or n)> ", stat, resultsMsg);

                Console.WriteLine();
                Console.WriteLine("    --------------------------------------------------------");

            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("The analysis is complete. Please review your results.");

            Console.WriteLine();
            Console.Write("The program will exit when you hit enter...");
            Console.ReadLine();
        }


        private static void writeResults(string prompt, FileStat stat, string resultsMsg)
        {
            Console.WriteLine();
            Console.Write(prompt);

            string response = Console.ReadLine();

            if (response == "y")
            {
                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "__RESULTS.txt")))
                {
                    sw.Write(resultsMsg);
                }

                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "FileSet.txt")))
                {
                    foreach (string file in stat.fileSet)
                    {
                        sw.WriteLine(file);
                    }
                }

                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "IndexNoFile.txt")))
                {
                    foreach (string record in stat.indexNoFile)
                    {
                        sw.WriteLine(record);
                    }
                }

                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "TotalMatches.txt")))
                {
                    foreach (string record in stat.totalMatches)
                    {
                        sw.WriteLine(record);
                    }
                }

                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "FileNoIndex.txt")))
                {
                    foreach (string file in stat.fileNoIndex)
                    {
                        sw.WriteLine(file);
                    }
                }

                using (StreamWriter sw = File.CreateText(Path.Combine(stat.resultsDir, "DuplicateFiles.txt")))
                {
                    foreach (string file in stat.duplicates)
                    {
                        sw.WriteLine(file);
                    }
                }
            }
            else if (response == "n")
            {
                return;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Not a valid entry. Please try again.");
                writeResults(prompt, stat, resultsMsg);
            }
        }
    }
}
