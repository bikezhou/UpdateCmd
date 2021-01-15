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
    /// <summary>
    /// 发布版本
    /// </summary>
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
            var dir = Path.Combine(options.SrcPath);
            if (!Directory.Exists(dir))
            {
                Console.WriteLine("File directory not exists.");
                return -1;
            }

            var uplog = string.Empty;
            var uplogFile = options.UpdateLogFile;
            if (File.Exists(uplogFile))
            {
                uplog = File.ReadAllText(uplogFile);
            }

            // ${RootPath}/${Name}
            var name = options.Name.ToLower();
            var rootPath = Path.Combine(_rootPath, name);

            // ${RootPath}/${Name}/update.json
            var updateJsonFile = Path.GetFullPath(Path.Combine(rootPath, "update.json"));

            // ${RootPath}/${Name}/${Version}
            var ver = options.Version;
            var verPath = Path.Combine(rootPath, ver.ToString());

            // ${RootPath}/${Name}/${Version}/files
            var filesPath = Path.Combine(verPath, "files");

            // ${RootPath}/${Name}/${Version}/version.json
            var publishJsonFile = Path.Combine(verPath, "version.json");

            var current = JsonHelper.DeserializeFromFile<UpdateDescription>(updateJsonFile) ?? new UpdateDescription();

            if (ver <= current.Version)
            {
                Console.WriteLine("New version lower than last version.");
                return -1;
            }

            var publish = new VersionDescription
            {
                Version = ver,
                MinSupport = options.IsMinSupport ? ver : current.MinSupport,
                UpdateLog = uplog
            };

            var srcFiles = new List<FileDescription>();
            foreach (var file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
            {
                var fn = file.Replace(dir, "").Replace('\\', '/').Trim('/');
                var fp = file;
                var md5 = FileHelper.GetMD5Hash(file);

                if (!options.IsFull)
                {
                    // 增量复制文件
                    var eq = current.Files.FirstOrDefault(a => a.Name.Equals(fn, StringComparison.OrdinalIgnoreCase) && a.Md5.Equals(md5));
                    if (eq != null)
                        continue;
                }

                var srcFile = new FileDescription
                {  
                    Name = fn,
                    FileUrl = fp,
                    Md5 = md5
                };

                srcFiles.Add(srcFile);
            }

            foreach (var file in srcFiles)
            {
                var dstFile = Path.Combine(filesPath, file.Name);
                var dstPath = Path.GetDirectoryName(dstFile);
                if (!Directory.Exists(dstPath))
                {
                    Directory.CreateDirectory(dstPath);
                }

                // 复制文件
                Console.WriteLine("Copy: {0}", file.Name);

                File.Copy(file.FileUrl, dstFile, true);

                var publishFile = new FileDescription
                {
                    Name = file.Name,
                    Md5 = file.Md5,
                    FileUrl = "/" + filesPath.Replace(rootPath, "").Replace('\\', '/').Trim('/')
                };
                publish.Files.Add(publishFile);

                // 更新update.json
                var eq = current.Files.FirstOrDefault(a => a.Name.Equals(file.Name, StringComparison.OrdinalIgnoreCase));
                if (eq == null)
                {
                    current.Files.Add(eq = new FileDescription());
                }

                eq.Name = publishFile.Name;
                eq.Md5 = publishFile.Md5;
                eq.FileUrl = publishFile.FileUrl;
            }

            JsonHelper.SerializeToFile(publishJsonFile, publish, true);

            current.Version = publish.Version;
            current.MinSupport = publish.MinSupport;
            current.UpdateLog = publish.UpdateLog;
            current.Updates.Insert(0, new UpdateItemDescription
            {
                Version = publish.Version,
                VersionFile = "/" + publishJsonFile.Replace(rootPath, "").Replace('\\', '/').Trim('/')
            });
            JsonHelper.SerializeToFile(updateJsonFile, current, true);

            if (File.Exists(updateJsonFile))
            {
                File.Copy(updateJsonFile, Path.Combine(Path.GetDirectoryName(updateJsonFile), $"update@{current.Version}.json"), true);
            }

            return 0;
        }
    }
}
