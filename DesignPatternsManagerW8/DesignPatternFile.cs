using System;

namespace DesignPatternsManagerW8
{
    public class DesignPatternFile
    {
        public int Id { get; set; }
        public String DesignPatternName { get; set; }
        public String DesignPatternType { get; set; }
        public String Path { get; set; }
        public String Description { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
    }
}
