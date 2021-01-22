using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UpdateCmd.Descriptions;
using UpdateCmd.Helpers;
using UpdateCmd.Options;

namespace UpdateCmd.Executors.Impl
{
    public class PublishExecutor : IExecutor<PublishOptions>
    {
        /// <summary>
        /// 发布根目录名称
        /// </summary>
        public static Func<string> PublishFolderName = () => "update";

        /// <summary>
        /// 升级描述文件名称
        /// </summary>
        public static Func<string> UpdateJsonFileName = () => "update.json";

        /// <summary>
        /// 升级列表描述文件名称
        /// </summary>
        public static Func<string> UplistJsonFileName = () => "uplist.json";

        /// <summary>
        /// 指定版本升级描述文件名称
        /// </summary>
        public static Func<Version, string> VersionUpdateJsonFileName = (version) => $"update@{version}.json";

        /// <summary>
        /// 版本文件目录名称
        /// </summary>
        public static Func<string> VersionFilesFolderName = () => "files";

        /// <summary>
        /// 版本描述文件
        /// </summary>
        public static Func<string> VersionFilesJsonFileName = () => "upfiles.json";


        public virtual void Execute(PublishOptions options)
        {
            try
            {
                options.Url = options.Url ?? new Uri($"file:///{Path.GetFullPath($"./{PublishFolderName()}")}");

                switch (options.Url.Scheme)
                {
                    case "file":
                        FileSchemePublish(options);
                        break;
                    default:
                        Console.WriteLine("not support scheme.");
                        break;
                }

                Console.WriteLine("publis complete.");

            }
            catch (Exception ex)
            {
                Console.WriteLine("error: {0}", ex.Message);
            }
        }

        /// <summary>
        /// 本地文件发布
        /// </summary>
        /// <param name="options"></param>
        private void FileSchemePublish(PublishOptions options)
        {
            // 发布名称
            var name = options.Name.ToLower();

            // 发布根目录: ${root}
            var rootPath = Path.GetFullPath(Path.Combine(options.Url.AbsolutePath, name));

            // 发布版本根目录: ${root}/${version}
            var versionPath = Path.Combine(rootPath, options.Version.ToString());

            // 发布版本文件目录: ${root}/${version}/files
            var versionFilePath = Path.Combine(versionPath, VersionFilesFolderName());

            // 升级描述文件: ${root}/update.json
            var fileUpdate = Path.Combine(rootPath, UpdateJsonFileName());

            // 升级列表描述文件: ${root}/uplist.json
            var fileUplist = Path.Combine(rootPath, UplistJsonFileName());

            // 版本描述文件: ${root}/${version}/upfiles.json
            var fileUpfiles = Path.Combine(versionPath, VersionFilesJsonFileName());

            // 指定版本升级描述文件: ${root}/update@${version}.json
            var fileUpdateVersion = Path.Combine(rootPath, VersionUpdateJsonFileName(options.Version));

            var update = JsonHelper.DeserializeFromFile<UpdateDescription>(fileUpdate) ?? new UpdateDescription();
            if (options.Version <= update.Version)
            {
                throw new Exception("current version lower than latest version.");
            }

            if (!Directory.Exists(options.Files))
            {
                throw new Exception("source files folder not exists. [--files]");
            }

            var sourceFiles = FindMatchFiles(options);

            // 查找已修改或新增文件
            var copyFiles = new List<FileDescription>();
            foreach (var sourceFile in sourceFiles)
            {
                var current = update.Files.Where(a => a.Name.Equals(sourceFile.Name) && a.Md5.Equals(sourceFile.Md5)).FirstOrDefault();
                if (current != null)
                {
                    continue;
                }

                copyFiles.Add(sourceFile);
            }

            if (copyFiles.Count == 0)
            {
                throw new Exception("not found any changed file.");
            }

            // 复制文件
            var upfiles = new UpdateDescription()
            {
                Version = options.Version,
                Lowest = options.Lowest ? options.Version : update.Lowest
            };

            foreach (var copyFile in copyFiles)
            {
                var relativeFilePath = copyFile.Url.Trim('/');
                var srcFile = Path.Combine(options.Files, relativeFilePath);
                var dstFile = Path.Combine(versionFilePath, relativeFilePath);

                var dstPath = Path.GetDirectoryName(dstFile);
                if (!Directory.Exists(dstPath))
                {
                    Directory.CreateDirectory(dstPath);
                }

                Console.WriteLine("copy file: {0}", copyFile.Name);
                File.Copy(srcFile, dstFile, true);

                var current = update.Files.Where(a => a.Name.Equals(copyFile.Name)).FirstOrDefault();
                if (current == null)
                {
                    current = new FileDescription();
                    update.Files.Add(current);
                }

                current.Name = copyFile.Name;
                current.Url = $"/{name}/{options.Version}/{VersionFilesFolderName()}/{relativeFilePath}";
                current.Md5 = copyFile.Md5;

                upfiles.Files.Add(current);
            }

            update.Version = upfiles.Version;
            update.Lowest = upfiles.Lowest;

            JsonHelper.SerializeToFile(fileUpfiles, upfiles);

            JsonHelper.SerializeToFile(fileUpdate, update);
            JsonHelper.SerializeToFile(fileUpdateVersion, update);

            var uplist = JsonHelper.DeserializeFromFile<UpdateListDescription>(fileUplist) ?? new UpdateListDescription()
            {
                Current = new UpdateListItemDescription()
            };

            uplist.Current.Version = update.Version;
            uplist.Current.Lowest = update.Lowest;
            uplist.Current.Url = $"/{name}/{UpdateJsonFileName()}";
            uplist.UpdateList.Insert(0, new UpdateListItemDescription
            {
                Version = update.Version,
                Lowest = update.Lowest,
                Url = $"/{name}/{VersionUpdateJsonFileName(update.Version)}"
            });

            JsonHelper.SerializeToFile(fileUplist, uplist);
        }

        /// <summary>
        /// 查找符合筛选条件的所有文件
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private List<FileDescription> FindMatchFiles(PublishOptions options)
        {
            var sourcePath = Path.GetFullPath(options.Files);
            if (string.IsNullOrWhiteSpace(sourcePath) || !Directory.Exists(sourcePath))
                return null;

            // include list
            var includes = new List<string>() { "*" };

            if (File.Exists(options.IncludeConf))
            {
                includes.AddRange(File.ReadAllLines(options.IncludeConf));
            }

            if (!string.IsNullOrWhiteSpace(options.Include))
            {
                includes.AddRange(options.Include.Split(new char[] { ';' }));
            }

            includes = includes.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()).Distinct().ToList();

            // exclude list 
            var excludes = new List<string>();

            if (File.Exists(options.ExcludeConf))
            {
                excludes.AddRange(File.ReadAllLines(options.ExcludeConf));
            }

            if (!string.IsNullOrWhiteSpace(options.Exclude))
            {
                excludes.AddRange(options.Exclude.Split(new char[] { ';' }));
            }

            excludes = excludes.Where(a => !string.IsNullOrWhiteSpace(a)).Select(a => a.Trim()).Distinct().ToList();

            var directories = new List<string> { sourcePath };

            // exclude directories
            var excludeDirectories = new List<string>();
            foreach (var exclude in excludes)
            {
                excludeDirectories.AddRange(Directory.GetDirectories(sourcePath, exclude, SearchOption.AllDirectories));
            }

            // include directories
            var includeDirectories = new List<string>();
            foreach (var include in includes)
            {
                includeDirectories.AddRange(Directory.GetDirectories(sourcePath, include, SearchOption.AllDirectories));
            }

            directories.AddRange(includeDirectories.Where(a => !excludeDirectories.Contains(a)));

            // exclude files
            var excludeFiles = new List<string>();
            foreach (var exclude in excludes)
            {
                foreach (var directory in directories)
                {
                    var c = exclude[exclude.Length - 1];
                    if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                        continue;

                    excludeFiles.AddRange(Directory.GetFiles(directory, exclude));
                }
            }

            // include files
            var includeFiles = new List<string>();
            foreach (var include in includes)
            {
                foreach (var directory in directories)
                {
                    includeFiles.AddRange(Directory.GetFiles(directory, include));
                }
            }

            includeFiles = includeFiles.Where(a => !excludeFiles.Contains(a)).ToList();

            // result file descriptions
            var results = new List<FileDescription>();
            foreach (var file in includeFiles)
            {
                var name = file.Replace(sourcePath, "").Replace('\\', '/').Trim('/');
                var url = "/" + name;
                var md5 = FileHelper.GetMD5Hash(file);
                var length = new FileInfo(file).Length;

                results.Add(new FileDescription
                {
                    Name = name,
                    Url = url,
                    Md5 = md5,
                    Length = length
                });
            }

            return results;
        }
    }
}
