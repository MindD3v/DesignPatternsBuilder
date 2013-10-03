using System;
using System.Collections.Generic;
using DesignPatternsCommonLibrary;

namespace DesignPatternsManager
{
    public class DesignPattensFileManagerImplementation : DesignPattensFileManager
    {
        public DesignPattensFileManagerImplementation()
        {
            ApplicationPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        }
        public override IEnumerable<string> GetFilesFromFolder(string folderName, IEnumerable<string> searchPattern)
        {
            throw new NotImplementedException();
        }

        public override bool FileExistsInFolder(string fileName, string folderName)
        {
            throw new NotImplementedException();
        }

        public override string CreateFile(string fileName, string path, string content)
        {
            throw new NotImplementedException();
        }

        public override string GetFolderPath(string folderName)
        {
            throw new NotImplementedException();
        }

        public override string DeleteFile(string fileName, string path)
        {
            throw new NotImplementedException();
        }

        public override string ReadFile(string fileName, string folderName)
        {
            throw new NotImplementedException();
        }
    }
}
