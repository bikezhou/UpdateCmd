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
        public virtual void Execute(PublishOptions options)
        {
            var sourceFiles = FindMatchFiles(options);

            JsonHelper.SerializeToFile("upfiles.json", sourceFiles ?? new List<FileDescription>());


        }

        /// <summary>
        /// Find all match files
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
                var name = Path.GetFileName(file);
                var url = "/" + file.Replace(sourcePath, "").Replace('\\', '/').Trim('/');
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
