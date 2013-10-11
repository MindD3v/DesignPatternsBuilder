
using System.Collections.Generic;

namespace MyActiveObject
{
    public class ActiveObjectEngine
    {
        private List<ICommand> _commands = new List<ICommand>();

        public void AddCommand(ICommand c)
        {
            _commands.Add(c);
        }
        public void Run()
        {
            while (_commands.Count > 0)
            {
                var c = _commands[0];
                _commands.RemoveAt(0);
                c.Execute();
            }
        }
    }
}
        