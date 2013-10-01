
using System;

namespace BuiltDesignPatternsTest.SingletonTest
{
    public class MySingletonLazy
    {
   
        private static readonly Lazy<MySingletonLazy> _instance
            = new Lazy<MySingletonLazy>(() => new MySingletonLazy());
   
       private MySingletonLazy()
       {
       }
    
  
       public static MySingletonLazy Instance
       {
           get
           {
               return _instance.Value;
           }
       }
  }
}
  