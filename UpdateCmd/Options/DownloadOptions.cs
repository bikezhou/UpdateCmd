using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    /// <summary>
    /// 下载发布版本文件
    /// </summary>
    [Verb("download", HelpText = "Download a published version.")]
    public class DownloadOptions
    {
        /// <summary>
        /// 指定下载版本, 默认最新版本
        /// </summary>
        [Option("version", Required = false, HelpText = "Set download version.")]
        public Version Version { get; set; }

        /// <summary>
        /// 完整下载所有文件
        /// </summary>
        [Option("full", Required = false, HelpText = "Set download all files.")]
        public bool IsFull { get; set; }

        /// <summary>
        /// 文件下载地址(update.json)
        /// </summary>
        [Option("url", Required = true, HelpText = "Set download url. smaple: http://download.domain.com/publish/update.json")]
        public string UpdateUrl { get; set; }
    }
}
