
using System;

namespace BuiltDesignPatternsTest.SingletonTest
{
  public sealed class MySingletonMultithreaded
  {
    private static volatile MySingletonMultithreaded _instance;
    private static object syncRoot = new Object();

    private MySingletonMultithreaded() {}

    public static MySingletonMultithreaded Instance
    {
      get 
      {
          if (_instance == null) 
          {
            lock (syncRoot) 
            {
                if (_instance == null) 
                  _instance = new MySingletonMultithreaded();
            }
          }

          return _instance;
      }
    }
  }
}
  