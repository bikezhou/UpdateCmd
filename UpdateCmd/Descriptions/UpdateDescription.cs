using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    public class UpdateDescription
    {
        public UpdateDescription()
        {
            Version = new Version();
            MinSupport = new Version();
            UpdateLog = string.Empty;
            Files = new List<FileDescription>();
            Updates = new List<UpdateItemDescription>();
        }

        public Version Version { get; set; }

        /// <summary>
        /// 最小支持版本
        /// </summary>
        public Version MinSupport { get; set; }

        /// <summary>
        /// 更新日志
        /// </summary>
        public string UpdateLog { get; set; }

        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileDescription> Files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<UpdateItemDescription> Updates { get; set; }
    }
}
