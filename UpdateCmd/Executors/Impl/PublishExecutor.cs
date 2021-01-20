using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateCmd.Options;

namespace UpdateCmd.Executors.Impl
{
    public class PublishExecutor : IExecutor<PublishOptions>
    {
        public virtual void Execute(PublishOptions options)
        {
            Console.WriteLine("Publis executed.");
        }
    }
}
