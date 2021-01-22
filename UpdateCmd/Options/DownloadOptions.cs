using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    [Verb("download", HelpText = "下载指定版本文件")]
    public class DownloadOptions
    {
        [Option("name", Required = true, HelpText = "已发布的升级名称")]
        public string Name { get; set; }

        [Option("url", Required = true, HelpText = "服务地址url")]
        public Uri Url { get; set; }

        [Option("version", HelpText = "指定下载版本")]
        public Version Version { get; set; }

        [Option("output", HelpText = "文件下载目录，默认为此程序目录下download目录")]
        public string Output { get; set; }
    }
}
