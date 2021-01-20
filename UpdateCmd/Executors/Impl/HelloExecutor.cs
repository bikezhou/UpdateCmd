using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateCmd.Options;

namespace UpdateCmd.Executors.Impl
{
    public class HelloExecutor : IExecutor<HelloOptions>
    {
        public void Execute(HelloOptions options)
        {
            Console.WriteLine("Hello world!");
        }
    }
}
