
namespace BuiltDesignPatternsTest.SingletonTest
{
  public sealed class MySingletonStaticInitialization
  {
    private static readonly MySingletonStaticInitialization _instance = new MySingletonStaticInitialization();

    private MySingletonStaticInitialization(){}

    public static MySingletonStaticInitialization Instance
    {
      get
      {
        return _instance;
      }
    }
  }
}
  