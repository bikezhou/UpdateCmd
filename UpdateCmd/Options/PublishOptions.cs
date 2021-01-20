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
        [Option("name", Required = true, HelpText = "发布升级包名称")]
        public string Name { get; set; }

        [Option("version", Required = true, HelpText = "发布升级包版本")]
        public Version Version { get; set; }

        [Option("files", Required = true, HelpText = "发布源文件所在目录")]
        public string Files { get; set; }

        [Option("lowest", HelpText = "标志当前版本为最低版本")]
        public bool Lowest { get; set; }

        [Option("include", HelpText = "包含文件/目录筛选配置，';'分割")]
        public string Include { get; set; }

        [Option("except", HelpText = "排除文件/目录筛选配置，';'分割，排除优先级高于包含")]
        public string Except { get; set; }

        [Option("include-conf", HelpText = "包含文件/目录筛选配置文件，换行符分割")]
        public string IncludeConf { get; set; }

        [Option("except-conf", HelpText = "排除文件/目录筛选配置文件，换行符分割")]
        public string ExceptConf { get; set; }

        [Option("url", HelpText = "服务地址url")]
        public string Url { get; set; }

        [Option("user", HelpText = "登录账号，需要登录时可用")]
        public string User { get; set; }

        [Option("pass", HelpText = "登录密码，需要登录时可用")]
        public string Pass { get; set; }
    }
}
