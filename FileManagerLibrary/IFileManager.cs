using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileManagerLibrary
{
    public interface IFileManager
    {
        IEnumerable<String> GetFilesFromFolder(String folderName, IEnumerable<String> searchPattern);
        Boolean FileExistsInFolder(String fileName, String folderName);
    }
}
