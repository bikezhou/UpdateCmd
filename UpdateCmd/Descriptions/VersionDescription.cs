using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    /// <summary>
    /// 更新版本描述
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
        [JsonProperty("version")]
        public Version Version { get; set; }

        /// <summary>
        /// 最小支持版本
        /// </summary>
        [JsonProperty("minSupport")]
        public Version MinSupport { get; set; }

        /// <summary>
        /// 更新内容描述
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        [JsonProperty("files")]
        public List<FileDescription> Files { get; set; }
    }
}
