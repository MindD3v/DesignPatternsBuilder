using System;
using System.IO;

namespace DesignPatternsManager
{
    public class FileWriter
    {
        public String Path { get; private set; }
        public String Content { get; private set; }

        public FileWriter(string path, string content)
        {
            Path = path;
            Content = content;
        }
        public void WriteFile()
        {
            if(Path != null && Content != null)
            {
                File.WriteAllText(Path, Content);
            }
        }

    }
}
