using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesignPatternsCommonLibrary;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;


namespace DesignPatternsManagerW8
{
    public class DesignPattensFileManager : IDesignPattensFileManager
    {
        #region IFileManager Members
        public async Task<bool> FileExistsInFolder(string fileName, string folderName)
        {
            var file = await GetFileFromFolderAsync(fileName, folderName);
            return file != null;
        }

        public async Task<IEnumerable<string>> GetFilesFromFolder(string folderName, IEnumerable<String> searchPattern)
        {
            var filesAsync = await GetFilesFromFolderAsync(folderName, searchPattern);

            var files = from f in filesAsync
                        select f.Name;

            return files.ToList();
        }
        public async Task<String> CreateFile(String fileName, String folderPath, String content)
        {
            var filePath = await CreateFileAsync(fileName, folderPath, content);
            return filePath;
        }
        public async Task<string> DeleteFile(string fileName, string path)
        {
            var deletedFilePath = await DeleteFileAsync(fileName, path);
            return deletedFilePath.Path;
        }
        public async Task<String> GetFolderPath(string folderName)
        {
            var folder = await GetStorageFolder(folderName);
            return folder.Path;
        }

        public async Task<String> ReadFile(String fileName, String folderName)
        {
            var fileContent = await ReadFileAsync(fileName, folderName);
            return fileContent;
        }
        public String GetDesignPatternsTemplatesPath()
        {
            return "DesignPatternsTemplates";
        }
        #endregion
        #region Specific Implementation
        private async Task<StorageFile> GetFileFromFolderAsync(String fileName, String folderName)
        {
            var allFiles = await GetFilesFromFolderAsync(folderName,null);

            var file = (from f in allFiles
                       where f.Name == fileName
                       select f).FirstOrDefault();

            return file;
        }

        private async Task<IEnumerable<StorageFile>> GetFilesFromFolderAsync(String folderName, IEnumerable<string> fileTypeFilter)
        {
            var folder = await GetStorageFolder(folderName);

            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery,fileTypeFilter)
                {
                    FolderDepth = FolderDepth.Deep
                };

            var queryResult = folder.CreateFileQueryWithOptions(queryOptions);
            var allFiles = await queryResult.GetFilesAsync();

            return allFiles;
        }
        
        private async Task<StorageFolder> GetStorageFolder(string folderName)
        {
            var applicationFolder = Package.Current.InstalledLocation;

            var queryOptions = new QueryOptions(CommonFolderQuery.DefaultQuery)
            {
                FolderDepth = FolderDepth.Deep
            };

            var queryResult = applicationFolder.CreateFolderQueryWithOptions(queryOptions);
            var allFolders = await queryResult.GetFoldersAsync();

            var folder = (from f in allFolders
                         where f.Name == folderName
                         select f).ToList();

            return folder.FirstOrDefault();
        }
        private async Task<String> CreateFileAsync(String fileName, String folderPath, String content)
        {
            var folder = await GetStorageFolder(folderPath);
            var createdFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(createdFile, content);

            return createdFile.Path;
        }
        private async Task<StorageFile> DeleteFileAsync(String fileName, String folderPath)
        {
            var folder = await GetStorageFolder(folderPath);
            var file = await folder.GetFileAsync(fileName);
            await file.DeleteAsync();

            return file;
        }
        private async Task<String> ReadFileAsync(String fileName, String folderName)
        {
            var folder = await GetStorageFolder(folderName);
            var file = await folder.GetFileAsync(fileName);

            var fileContent = await FileIO.ReadTextAsync(file);

            return fileContent;
        }
        #endregion
    }
}
