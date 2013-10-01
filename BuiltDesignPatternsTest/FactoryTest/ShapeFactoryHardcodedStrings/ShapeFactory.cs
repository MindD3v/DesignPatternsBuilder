
        namespace BuiltDesignPatternsTest.FactoryTest.ShapeFactoryHardcodedStrings
        {
            public class ShapeFactory : IShapeFactory
            {
                public IShape Make(string name)
                {
                    
          if(name.Equals("Circle"))
            return new Circle();
        
          if(name.Equals("Square"))
            return new Square();
        
                    
                    return null;
                }
            }
        }
        