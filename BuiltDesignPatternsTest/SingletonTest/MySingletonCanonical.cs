
namespace BuiltDesignPatternsTest.SingletonTest
{
  public class MySingletonCanonical
  {
      private static MySingletonCanonical _instance;

      private MySingletonCanonical() {}

      public static MySingletonCanonical Instance
      {
          get 
          {
              if (_instance == null)
              {
                  _instance = new MySingletonCanonical();
              }
              return _instance;
          }
      }
  }
}
  