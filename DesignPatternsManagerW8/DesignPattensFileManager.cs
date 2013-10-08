using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Pickers;


namespace DesignPatternsManagerW8
{
    public class DesignPattensFileManager : IDesignPattensFileManager
    {
        public async Task<IEnumerable<StorageFile>> GetFilesFromFolder(StorageFolder folder, IEnumerable<string> searchPattern)
        {
            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery, searchPattern)
            {
                FolderDepth = FolderDepth.Deep
            };

            var queryResult = folder.CreateFileQueryWithOptions(queryOptions);
            var allFiles = await queryResult.GetFilesAsync();

            return allFiles;
        }

        public async Task<bool> FileExistsInFolder(string fileName, StorageFolder folder)
        {
            var allFiles = await GetFilesFromFolder(folder, null);

            var file = (from f in allFiles
                        where f.Name == fileName
                        select f).FirstOrDefault();

            return file != null;
        }

        public async Task<StorageFile> CreateFile(string fileName, StorageFolder folder, string content)
        {
            var createdFile = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(createdFile, content);

            return createdFile;
        }

        public async Task<string> DeleteFile(String fileName, StorageFolder folder)
        {
            var file = await folder.GetFileAsync(fileName);
            await file.DeleteAsync();

            return file.Path;
        }

        public async Task<string> ReadFile(string fileName, StorageFolder folder)
        {
            var file = await folder.GetFileAsync(fileName);

            var fileContent = await FileIO.ReadTextAsync(file);

            return fileContent;
        }

        public  async Task<StorageFolder> GetDesignPatternsTemplatesPath()
        {
            var folder = await GetApplicationStorageFolder("DesignPatternsTemplates");
            return folder;
        }

        public async Task<StorageFolder> GetApplicationStorageFolder(string folderName)
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
    }
}
