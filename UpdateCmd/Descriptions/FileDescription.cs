using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    /// <summary>
    /// 文件描述
    /// </summary>
    public class FileDescription
    {
        /// <summary>
        /// 文件名(相对路径)
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// 文件md5值
        /// </summary>
        [JsonProperty("md5")]
        public string Md5 { get; set; }
    }
}
