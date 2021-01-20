using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Descriptions
{
    public class UpdateListDescription
    {
        [JsonProperty("current")]
        public UpdateListItemDescription Current { get; set; }

        [JsonProperty("uplist")]
        public IList<UpdateListItemDescription> UpdateList { get; set; }

        public UpdateListDescription()
        {
            UpdateList = new List<UpdateListItemDescription>();
        }
    }
}
