using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace console
{
    public class Parser
    {
        private readonly string _folder;

        private List<string> _folders;
        private Dictionary<string,string> _files;
        public Parser(string folder)
        {
            _folder = folder;
            _folders = new List<string>();
            _files = new Dictionary<string, string>();
        }

        public Dictionary<string,string> GetFiles()
        {
            GetFiles(GetTemplateLocation());
            return _files;
        }

        private void GetFiles(string parentFolder)
        {
            AddFilesFromDirectory(new DirectoryInfo(parentFolder));

            foreach (var directory in new DirectoryInfo(parentFolder).GetDirectories())
            {
                AddFilesFromDirectory(directory);

                if (directory.GetDirectories().Length > 0)
                {
                    GetFiles(directory.FullName);
                }
            }
        }

        private void AddFilesFromDirectory(DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles())
            {
                if (!_files.ContainsKey(GetPathInDestinationProject(file.FullName)))
                {
                    using (var reader = new StreamReader(file.FullName))
                    {
                        _files.Add(GetPathInDestinationProject(file.FullName), reader.ReadToEnd());
                    }
                }
            }
        }

        public List<string> GetDirectories()
        {
            GetDirectories(GetTemplateLocation());
            return _folders;
        }

        private void GetDirectories(string parent)
        {
            foreach (var directory in new DirectoryInfo(parent).GetDirectories())
            {
                if (!_folders.Contains(GetPathInDestinationProject(directory.FullName)))
                    _folders.Add(GetPathInDestinationProject(directory.FullName));

                if(directory.GetDirectories().Length>0)
                {
                    GetDirectories(directory.FullName);
                }
            }
        }

        private string GetPathInDestinationProject(string directory)
        {
            return directory.Replace(GetTemplateLocation(), "");
        }

        private string GetTemplateLocation()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location.Replace(@"\bin\Debug","")), _folder);
        }

    }
}
