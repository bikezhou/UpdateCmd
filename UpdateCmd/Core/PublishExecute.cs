using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UpdateCmd.Descriptions;
using UpdateCmd.Helpers;
using UpdateCmd.Options;

namespace UpdateCmd.Core
{
    internal class PublishExecute : IExecute<PublishOptions>
    {
        private string _rootPath;

        public PublishExecute()
        {
            _rootPath = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "update"));
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
        }

        public int Execute(PublishOptions options)
        {
            if (!Directory.Exists(options.Directory))
            {
                Console.WriteLine("File directory not exists.");
                return -1;
            }

            // 当前升级项目目录
            var curRootPath = Path.Combine(_rootPath, options.Name.ToLower());
            if (!Directory.Exists(curRootPath))
            {
                Directory.CreateDirectory(curRootPath);
            }

            var curVersionFile = Path.Combine(curRootPath, $"update{(string.IsNullOrEmpty(options.Branch) ? "" : $".{options.Branch.ToLower()}")}.json");
            var curVersion = new VersionDescription
            {
                Version = new Version(),
                MinSupport = new Version()
            };

            if (File.Exists(curVersionFile))
            {
                curVersion = JsonHelper.DeserializeFromFile<VersionDescription>(curVersionFile);
            }

            if (options.Version <= curVersion.Version)
            {
                Console.WriteLine("New version lower than last version.");
                return -1;
            }

            var srcPath = Path.GetFullPath(options.Directory);
            var srcVersion = new VersionDescription
            {
                Version = options.Version,
                MinSupport = options.Force ? options.Version : curVersion.MinSupport,
            };

            var srcFiles = new List<FileDescription>();

            foreach (var file in Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories))
            {
                var name = file.Replace(srcPath, "").Replace('\\', '/').Trim('/');
                var md5 = FileHelper.GetMD5Hash(file);

                var curFile = curVersion.Files.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && a.Md5.Equals(md5));

                // 先前版本中已存在且md5相同,跳过
                if (curFile != null)
                    continue;

                var fileDescription = new FileDescription
                {
                    Name = name,
                    Path = file,
                    Md5 = md5
                };

                srcFiles.Add(fileDescription);
            }

            // 复制文件
            foreach (var fdesc in srcFiles)
            {
                var srcFile = fdesc.Path;
                var dstFile = Path.Combine(curRootPath, srcVersion.Version.ToString(), "files", fdesc.Name);

                var dstPath = Path.GetDirectoryName(dstFile);
                if (!Directory.Exists(dstPath))
                {
                    Directory.CreateDirectory(dstPath);
                }

                File.Copy(srcFile, dstFile, true);

                srcVersion.Files.Add(new FileDescription
                {
                    Name = fdesc.Name,
                    Path = "files/",
                    Md5 = fdesc.Md5
                });
            }

            var srcVersionFile = Path.Combine(curRootPath, srcVersion.Version.ToString(), "version.json");
            JsonHelper.SerializeToFile(srcVersionFile, srcVersion, true);

            return 0;
        }
    }
}
