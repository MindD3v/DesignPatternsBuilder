using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileManagerLibrary;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;


namespace DesignPatternsManagerW8
{
    public class FileManager : IFileManager
    {
        public String ApplicationPath { get; private set; }

        public FileManager()
        {
            ApplicationPath = Package.Current.InstalledLocation.Path;
        }

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
            var applicationFolder = Package.Current.InstalledLocation;
            var folder = await applicationFolder.GetFolderAsync(folderName);

            var queryOptions = new QueryOptions(CommonFileQuery.DefaultQuery,fileTypeFilter)
                {
                    FolderDepth = FolderDepth.Shallow
                };

            var queryResult = folder.CreateFileQueryWithOptions(queryOptions);
            var allFiles = await queryResult.GetFilesAsync();

            return allFiles;
        }

        public bool FileExistsInFolder(string fileName, string folderName)
        {
            var file = GetFileFromFolderAsync(fileName, folderName).Result;
            return  file != null;
        }

        public IEnumerable<string> GetFilesFromFolder(string folderName, IEnumerable<String> searchPattern)
        {
            var filesAsync = GetFilesFromFolderAsync(folderName, searchPattern).Result;

            var files = from f in filesAsync
                        select f.Path;

            return files.ToList();
        }
    }
}
