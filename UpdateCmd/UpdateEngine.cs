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
        private IExecute<DownloadOptions> _download;

        public UpdateEngine()
        {
            _hello = new HelloExecute();
            _publish = new PublishExecute();
            _download = new DownloadExecute();
        }

        public void Dispose()
        {
        }

        internal void Execute(string[] args)
        {
            Parser.Default.ParseArguments<HelloOptions, PublishOptions, DownloadOptions>(args)
                .WithParsed<HelloOptions>(ExecuteHello)
                .WithParsed<PublishOptions>(ExecutePublish)
                .WithParsed<DownloadOptions>(ExecuteDownload);
        }

        private void ExecuteHello(HelloOptions options)
        {
            _hello.Execute(options);
        }

        private void ExecutePublish(PublishOptions options)
        {
            _publish.Execute(options);
        }

        private void ExecuteDownload(DownloadOptions options)
        {
            _download.Execute(options);
        }
    }
}
