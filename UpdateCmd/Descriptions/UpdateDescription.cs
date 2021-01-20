using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    public class UpdateDescription
    {
        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("lowest")]
        public Version Lowest { get; set; }

        [JsonProperty("files")]
        public IList<FileDescription> Files { get; set; }

        public UpdateDescription()
        {
            Files = new List<FileDescription>();
        }
    }
}
