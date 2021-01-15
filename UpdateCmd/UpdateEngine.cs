using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UpdateCmd.Core;
using UpdateCmd.Options;

namespace UpdateCmd
{
    /// <summary>
    /// 更新指令执行引擎
    /// </summary>
    public class UpdateEngine : IDisposable
    {
        private IExecute<HelloOptions> _hello;
        private IExecute<PublishOptions> _publish;

        public UpdateEngine()
        {
            _hello = new HelloExecute();
            _publish = new PublishExecute();
        }

        public void Dispose()
        {
        }

        internal void Execute(string[] args)
        {
            Parser.Default.ParseArguments<HelloOptions, PublishOptions>(args)
                .WithParsed<HelloOptions>(ExecuteHello)
                .WithParsed<PublishOptions>(ExecutePublish);
        }

        private void ExecuteHello(HelloOptions options)
        {
            _hello.Execute(options);
        }

        private void ExecutePublish(PublishOptions options)
        {
            _publish.Execute(options);
        }
    }
}
