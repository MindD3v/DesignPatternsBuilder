using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DesignPatternsCommonLibrary
{
    public interface IDesignPattensFileManager
    {
        Task<IEnumerable<String>> GetFilesFromFolder(String folderName, IEnumerable<String> searchPattern);
        Task<Boolean> FileExistsInFolder(String fileName, String folderName);
        Task<String> CreateFile(String fileName, String path,String content);
        Task<String> GetFolderPath(string folderName);
        Task<String> DeleteFile(String fileName, String folderName);
        Task<String> ReadFile(String fileName, String folderName);
        String GetDesignPatternsTemplatesPath();
    }
}
