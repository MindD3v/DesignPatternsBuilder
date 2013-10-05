using System;

namespace DesignPatternsCommonLibrary
{
    public class DesignPatternFile
    {
        public int Id { get; set; }
        public String DesignPatternName { get; set; }
        public DesignPatternType DesignPatternType { get; set; }
        public String Path { get; set; }
        public String Description { get; set; }
    }
}
