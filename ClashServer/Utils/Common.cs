using System.IO;

namespace ClashServer.Utils
{
    public static class Common
    {
        public static string GetFolderStoreImage()
        {
            var folderName = FolderName;
            return Path.Combine(Directory.GetCurrentDirectory(), folderName);
        }

        public static string FolderName = "Resources";
    }
}