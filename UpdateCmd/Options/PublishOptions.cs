using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    [Verb("publish", HelpText = "Publish new version.")]
    public class PublishOptions
    {
        [Option("name", Required = true, HelpText = "Required, Set name.")]
        public string Name { get; set; }

        [Option("version", Required = true, HelpText = "Required, Set version.")]
        public Version Version { get; set; }

        [Option("files", Required = true, HelpText = "Required, Set source files directory.")]
        public string Files { get; set; }

        [Option("lowest", HelpText = "Set this version is lowest support.")]
        public bool Lowest { get; set; }

        [Option("include", HelpText = "Set include file filter.")]
        public string Include { get; set; }

        [Option("except", HelpText = "Set except file filter.")]
        public string Except { get; set; }

        [Option("include-conf", HelpText = "Set include file filter config file.")]
        public string IncludeConf { get; set; }

        [Option("except-conf", HelpText = "Set except file filter config file.")]
        public string ExceptConf { get; set; }

        [Option("protocol", HelpText = "Set protocol<file|ftp|tcp|udp|http>.")]
        public string Protocol { get; set; }

        [Option("root", HelpText = "Set root directory, valid when protocol=file.")]
        public string Root { get; set; }

        [Option("server", HelpText = "Set connect server, valid when protocol=ftp|tcp|udp|http.")]
        public string Server { get; set; }

        [Option("port", HelpText = "Set connect server port.")]
        public ushort Port { get; set; }

        [Option("user", HelpText = "Set connect username when need.")]
        public string User { get; set; }

        [Option("pass", HelpText = "Set connect password when need.")]
        public string Pass { get; set; }
    }
}
