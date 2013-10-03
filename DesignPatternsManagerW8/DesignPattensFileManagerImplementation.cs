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
    public class DesignPattensFileManagerImplementation : DesignPattensFileManager
    {
        public DesignPattensFileManagerImplementation()
        {
            ApplicationPath = Package.Current.InstalledLocation.Path;
            DesignPatternsManagerPath = "DesignPatternsCommonLibrary";
            DesignPatternsTemplatesPath = "DesignPatternsTemplates";
        }
        #region IFileManager Members
        public override bool FileExistsInFolder(string fileName, string folderName)
        {
            var file = GetFileFromFolderAsync(fileName, folderName).Result;
            return file != null;
        }

        public override IEnumerable<string> GetFilesFromFolder(string folderName, IEnumerable<String> searchPattern)
        {
            var filesAsync = GetFilesFromFolderAsync(folderName, searchPattern).Result;

            var files = from f in filesAsync
                        select f.Name;

            return files.ToList();
        }
        public override String CreateFile(String fileName, String folderPath, String content)
        {
            var filePath = CreateFileAsync(fileName, folderPath, content).Result;
            return filePath;
        }
        public override string DeleteFile(string fileName, string path)
        {
            var deletedFilePath = DeleteFileAsync(fileName, path).Result;
            return deletedFilePath.Path;
        }
        public override String GetFolderPath(string folderName)
        {
            var folder = GetStorageFolder(folderName).Result;
            return folder.Path;
        }

        public override String ReadFile(String fileName, String folderName)
        {
            var fileContent = ReadFileAsync(fileName, folderName).Result;
            return fileContent;
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
            var allFolders = (await queryResult.GetFoldersAsync()).ToList();

            var folder = from f in allFolders
                         where f.Name == folderName
                         select f;

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
