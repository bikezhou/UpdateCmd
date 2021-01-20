using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateCmd.Executors;
using UpdateCmd.Executors.Impl;
using UpdateCmd.Options;

namespace UpdateCmd
{
    public class UpdateCmdEngine : IDisposable
    {
        private readonly IExecutor<HelloOptions> _helloExecutor;
        private readonly IExecutor<PublishOptions> _publishExecutor;

        public UpdateCmdEngine()
        {
            _helloExecutor = new HelloExecutor();
            _publishExecutor = new PublishExecutor();
        }

        public void Execute(string[] args)
        {
            Parser.Default.ParseArguments<HelloOptions, PublishOptions>(args)
                .WithParsed<HelloOptions>(HelloExecute)
                .WithParsed<PublishOptions>(PublishExecute);
        }

        private void PublishExecute(PublishOptions options)
        {
            _publishExecutor?.Execute(options);
        }

        private void HelloExecute(HelloOptions options)
        {
            _helloExecutor?.Execute(options);
        }

        public void Dispose()
        {
        }
    }
}
