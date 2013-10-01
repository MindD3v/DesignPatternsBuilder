
        namespace BuiltDesignPatternsTest.FactoryTest.ShapeFactoryExplicit
        {
            public class ShapeFactory : IShapeFactory
            {
                
          public IShape MakeCircle()
          {
              return new Circle();
          }
        
          public IShape MakeSquare()
          {
              return new Square();
          }
        
            }
        }
        