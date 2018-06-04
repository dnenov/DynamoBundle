using ByteSizeLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Archilizer_Purge
{
    public class DeleteBackups : IDisposable
    {
        private static int files = 0;
        private static double lengths = 0.0;

        public static DeleteBackups Instance { get; private set; }
        static DeleteBackups()
        {
            Instance = new DeleteBackups();
        }
        /// <summary>
        /// Deletes those pesky *.0023.rvt files
        /// </summary>
        public static void Delete()
        {
            // TO-DO: Make it Recursive
            using (var fbd = new OpenFileDialog())
            {
                fbd.Multiselect = false;

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName))
                {
                    DeleteRecursive(Path.GetDirectoryName(fbd.FileName));
                }
            }
            var b = ByteSize.FromBytes(lengths);
            MessageBox.Show(String.Format("{0} files with a total of {1} deleted.", files.ToString(), b.ToString("MB")));
        }
        /// <summary>
        /// Recursive method to delete files in sub directories
        /// </summary>
        /// <param name="v"></param>
        private static void DeleteRecursive(string v)
        {
            var dir = new DirectoryInfo(v);

            var directories = CustomSearcher.GetDirectories(dir.FullName);

            foreach(var directory in directories)
            {
                DeleteRecursive(directory);
            }

            foreach (var file in dir.EnumerateFiles("*.0???.*"))
            {
                lengths += file.Length;
                files++;
                file.Delete();
            }
        }
        public void Dispose()
        {
            files = 0;
            lengths = 0;
        }
        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }
    }

    public class CustomSearcher
    {
        public static List<string> GetDirectories(string path, string searchPattern = "*",
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (searchOption == SearchOption.TopDirectoryOnly)
                return Directory.GetDirectories(path, searchPattern).ToList();

            var directories = new List<string>(GetDirectories(path, searchPattern));

            for (var i = 0; i < directories.Count; i++)
                directories.AddRange(GetDirectories(directories[i], searchPattern));

            return directories;
        }

        private static List<string> GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern).ToList();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<string>();
            }
        }
    }
}