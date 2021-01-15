using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    /// <summary>
    /// publish new version
    /// </summary>
    [Verb("publish", HelpText = "Publish a new version.")]
    public class PublishOptions
    {
        [Option('n', "name", Required = true, HelpText = "Set publish name.")]
        public string Name { get; set; }

        [Option('b', "branch", Required = false, HelpText = "Set publish branch.")]
        public string Branch { get; set; }

        [Option('v', "version", Required = true, HelpText = "Set publish version.")]
        public Version Version { get; set; }

        [Option('d', "dir", Required = true, HelpText = "Set publish files directory.")]
        public string Directory { get; set; }

        [Option('f', "force", Required = false, HelpText = "Set this version is minsupport.")]
        public bool Force { get; set; }
    }
}
