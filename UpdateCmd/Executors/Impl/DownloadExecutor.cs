using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UpdateCmd.Descriptions;
using UpdateCmd.Helpers;
using UpdateCmd.Options;

namespace UpdateCmd.Executors.Impl
{
    public class DownloadExecutor : IExecutor<DownloadOptions>
    {
        /// <summary>
        /// 升级列表描述文件名称
        /// </summary>
        public static Func<string> UplistJsonFileName = () => "uplist.json";

        public void Execute(DownloadOptions options)
        {
            try
            {
                switch (options.Url.Scheme)
                {
                    case "file":
                        FileSchemeDownload(options);
                        break;
                    default:
                        break;
                }

                Console.WriteLine("download complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
            }
        }

        private void FileSchemeDownload(DownloadOptions options)
        {
            var name = options.Name.ToLower();

            var rootPath = Path.GetFullPath(Path.Combine(options.Url.AbsolutePath));

            var nameRootPath = Path.GetFullPath(Path.Combine(rootPath, name));

            if (!Directory.Exists(nameRootPath))
            {
                throw new Exception("publish name not found.");
            }

            var fileUplist = Path.Combine(nameRootPath, UplistJsonFileName());

            var uplist = JsonHelper.DeserializeFromFile<UpdateListDescription>(fileUplist);

            if (uplist == null)
            {
                throw new Exception("publish uplist not found.");
            }

            var upitem = uplist.Current;
            if (options.Version != null)
            {
                upitem = uplist.UpdateList.FirstOrDefault(a => a.Version == options.Version);
                if (upitem == null)
                {
                    throw new Exception("appoint publish version not found.");
                }
            }

            var output = Path.GetFullPath("./download");

            if (!string.IsNullOrWhiteSpace(options.Output))
            {
                output = Path.GetFullPath(options.Output);
            }

            var nameOutput = Path.Combine(output, name);

            var fileUpdate = Path.Combine(rootPath, upitem.Url.Trim('/'));

            var update = JsonHelper.DeserializeFromFile<UpdateDescription>(fileUpdate);

            if (update == null)
            {
                throw new Exception("appoint publish version not found.");
            }

            JsonHelper.SerializeToFile(Path.Combine(output, $"update.{name}.json"), update);

            foreach (var file in update.Files)
            {
                var srcfile = Path.GetFullPath(Path.Combine(rootPath, file.Url.Trim('/')));
                var dstfile = Path.Combine(nameOutput, file.Name);

                var dstpath = Path.GetDirectoryName(dstfile);
                if (!Directory.Exists(dstpath))
                {
                    Directory.CreateDirectory(dstpath);
                }
                Console.WriteLine("download file: {0}", file.Name);
                File.Copy(srcfile, dstfile);
            }
        }
    }
}
