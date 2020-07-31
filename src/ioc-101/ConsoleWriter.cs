using System;

namespace ioc_101
{
    public interface IWriter
    {
        void WriteLine(string line);
    }

    public class ConsoleWriter : IWriter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }


}