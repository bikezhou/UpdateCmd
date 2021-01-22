using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    public class UpdateListItemDescription
    {
        [JsonProperty("version")]
        public Version Version { get; set; }

        [JsonProperty("lowest")]
        public Version Lowest { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public UpdateListItemDescription()
        {
            Version = new Version();
            Lowest = new Version();
        }
    }
}
