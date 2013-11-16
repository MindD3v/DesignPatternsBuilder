using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace DesignPatternsManagerW8
{
    public interface IDesignPattensFileManager
    {
        Task<IEnumerable<StorageFile>> GetFilesFromFolder(StorageFolder folder, IEnumerable<String> searchPattern);
        Task<Boolean> FileExistsInFolder(String fileName, StorageFolder folder);
        Task<StorageFile> CreateFile(String fileName, StorageFolder folder, String content);
        Task<string> DeleteFile(String fileName, StorageFolder folder);
        Task<String> ReadFile(String fileName, StorageFolder folder);
        Task<StorageFolder> GetApplicationStorageFolder(string folderName);
        Task<StorageFolder> GetDesignPatternsTemplatesPath();
    }
}
