using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Options
{
    /// <summary>
    /// 发布新版本
    /// </summary>
    [Verb("publish", HelpText = "Publish a new version.")]
    public class PublishOptions
    {
        /// <summary>
        /// 发布项目名称
        /// </summary>
        [Option("name", Required = true, HelpText = "Set publish name.")]
        public string Name { get; set; }

        /// <summary>
        /// 发布版本
        /// </summary>
        [Option("version", Required = true, HelpText = "Set publish version.")]
        public Version Version { get; set; }

        /// <summary>
        /// 是否最小支持版本
        /// </summary>
        [Option("min", Required = false, HelpText = "Set this version is minsupport.")]
        public bool IsMinSupport { get; set; }

        /// <summary>
        /// 发布文件完整复制
        /// </summary>
        [Option("full", Required = false, HelpText = "Set publish file full copy.")]
        public bool IsFull { get; set; }

        /// <summary>
        /// 发布分支名称
        /// </summary>
        [Option("branch", Required = false, HelpText = "Set publish branch.")]
        public string Branch { get; set; }

        /// <summary>
        /// 分支依赖基准版本
        /// </summary>
        [Option("base", Required = false, HelpText = "Set publish branch base.")]
        public string BranchBase { get; set; }

        /// <summary>
        /// 发布原始文件目录
        /// </summary>
        [Option("dir", Required = true, HelpText = "Set publish files directory.")]
        public string Directory { get; set; }

        /// <summary>
        /// 发布版本更新日志文件
        /// </summary>
        [Option("logfile", Required = false, HelpText = "Set publish version update log file.")]
        public string UpdateLogFile { get; set; }
    }
}
