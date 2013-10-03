using System;
using System.Collections.Generic;

namespace DesignPatternsCommonLibrary
{
    public interface IFileManager
    {
        IEnumerable<String> GetFilesFromFolder(String folderName, IEnumerable<String> searchPattern);
        Boolean FileExistsInFolder(String fileName, String folderName);
        String CreateFile(String fileName, String path,String content);
        String GetFolderPath(string folderName);
        String DeleteFile(String fileName, String folderName);
        String ReadFile(String fileName, String folderName);
    }
}
