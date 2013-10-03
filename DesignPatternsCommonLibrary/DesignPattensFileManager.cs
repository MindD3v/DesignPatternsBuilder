using System;
using System.Collections.Generic;

namespace DesignPatternsCommonLibrary
{
    public abstract class DesignPattensFileManager : IFileManager
    {
        public String ApplicationPath { get; protected set; }
        public String DesignPatternsManagerPath { get; protected set; }
        public String DesignPatternsTemplatesPath { get; protected set; }

        public abstract IEnumerable<string> GetFilesFromFolder(string folderName, IEnumerable<string> searchPattern);
        public abstract bool FileExistsInFolder(string fileName, string folderName);
        public abstract String CreateFile(String fileName, String path, String content);
        public abstract String GetFolderPath(string folderName);
        public abstract String DeleteFile(String fileName, String path);
        public abstract String ReadFile(String fileName, String folderName);
    }
}
