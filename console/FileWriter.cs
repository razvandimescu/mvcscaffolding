using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace console
{
    public class FileWriter
    {
        private readonly string _applicationName;
        private readonly string _outputLocation;
        private readonly Parser _parser;
        private readonly ConstsReplacer _replacer;

        public FileWriter(string applicationName, string outputLocation, Parser parser, ConstsReplacer replacer)
        {
            _applicationName = applicationName;
            _outputLocation = outputLocation;
            _parser = parser;
            _replacer = replacer;
        }

        public void CreateProject()
        {
            
            foreach (string folder in _parser.GetDirectories())
            {
                CreateNonExistentFolders(_replacer.replace(folder));
            }

            foreach (KeyValuePair<string,string> file in _parser.GetFiles())
            {
                CreateNonExistendFiles(file);
            }

            
        }

        private void CreateNonExistendFiles(KeyValuePair<string, string> file)
        {
            if (File.Exists(DestinationFileName(file))) return;

            using (var write = new StreamWriter(DestinationFileName(file)))
            {
                write.WriteLine(_replacer.replace(file.Value));
                write.Flush();
            }
        }

        private string DestinationFileName(KeyValuePair<string, string> file)
        {
            string outputFile = _replacer.replace(file.Key);
            return Path.Combine(_outputLocation, outputFile);
        }

        private void CreateNonExistentFolders(string file)
        {
            if (!Directory.Exists(Path.Combine(_outputLocation, file)))
            {
                Directory.CreateDirectory(Path.Combine(_outputLocation, file));
            }
        }
    }
}
