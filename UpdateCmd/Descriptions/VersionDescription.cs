using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    /// <summary>
    /// 版本更新描述
    /// </summary>
    public class VersionDescription
    {
        public VersionDescription()
        {
            Files = new List<FileDescription>();
        }

        /// <summary>
        /// 更新版本
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// 最小支持版本
        /// </summary>
        public Version MinSupport { get; set; }

        /// <summary>
        /// 更新内容描述
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileDescription> Files { get; set; }
    }
}
