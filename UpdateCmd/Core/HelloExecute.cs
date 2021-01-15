using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateCmd.Options;

namespace UpdateCmd.Core
{
    internal class HelloExecute : IExecute<HelloOptions>
    {
        public int Execute(HelloOptions options)
        {
            Console.WriteLine("Say: " + options.Hello);
            return 0;
        }
    }
}
