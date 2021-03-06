VennDexer
=========

Compare File Set to CSV Index

###Requirements

Needs the [`SigChecker`](https://github.com/tmoore82/SigChecker) library or a copy of its CheckSignature method.

###Use Case:

Client is changing CRM vendors and needs to upload their attachments. Client provides new vendor with 1) CSV index of attachments from old system and 2) Collection of folders, possibly compressed, containing attachment files.

###What it does

VennDexer (working title) is a .NET library (C#) that accepts a config file as input and then takes the following steps:

1. Extracts files from the compressed folders (Note: If the files are not compressed, this step is skipped)
2. Generates a list of the complete file set across all directories and sub directories
3. Extracts the file names from the provided index
4. Delivers the following data back to the consuming application as a custom object:
  - Index record count (number of files we should have)
  - File count (number of files we actually have, excluding duplicates)
  - List of files, compiled from source directories. This is not a copy of the index.
  - List of matches (full lines from CSV)
  - List of index records that do not match a file (full lines from CSV)
  - List of files that do not have an index
  - List of duplicate files

###Bonus Consumer App
        
This solution also includes a sample consuming application for the console that just takes the location of the config file as input. It creates a VennDexer.FileStat object to receive the results, and then passes the location of the config file to Venngine.crank. The library does the rest.
