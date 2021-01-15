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
            Updates = new List<KeyValuePair<Version, string>>();
        }

        public Version Version { get; set; }

        public List<KeyValuePair<Version, string>> Updates { get; set; }
    }
}
