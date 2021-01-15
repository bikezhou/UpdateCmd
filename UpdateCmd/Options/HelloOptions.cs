using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    [Verb("hello", HelpText = "Hello verb.")]
    public class HelloOptions
    {
        [Option("hello", Required = false, HelpText = "Set hello message.")]
        public string Hello { get; set; }
    }
}
