﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UpdateCmd.Helpers
{
    public class JsonHelper
    {
        private static JsonSerializer _serializer;

        static JsonHelper()
        {
            _serializer = new JsonSerializer();
            _serializer.Converters.Add(new VersionConverter());

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new VersionConverter());

            JsonConvert.DefaultSettings = () =>
            {
                return settings;
            };
        }

        /// <summary>
        /// Json序列化
        /// </summary>
        /// <param name="value">序列化对象</param>
        /// <param name="indented">是否格式化</param>
        /// <returns></returns>
        public static string SerializeObject(object value, bool indented = false)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None);
        }

        public static object DeserializeObject(string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// Json反序列化
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="value">Json字符串</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Json序列化到文件
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="value">序列化对象</param>
        /// <param name="indented">是否格式化</param>
        public static void SerializeToFile(string filename, object value, bool indented = true)
        {
            if (value == null)
                return;

            var fullfilename = Path.GetFullPath(filename);
            ;
            var fullpath = Path.GetDirectoryName(fullfilename);
            if (!Directory.Exists(fullpath))
            {
                Directory.CreateDirectory(fullpath);
            }

            using (var fs = new FileStream(fullfilename, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (var sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        jw.Formatting = indented ? Formatting.Indented : Formatting.None;
                        _serializer.Serialize(jw, value, value.GetType());
                        jw.Flush();
                    }
                }
            }
        }

        public static object DeserializeFromFile(string filename)
        {
            var fullfilename = Path.GetFullPath(filename);

            if (!File.Exists(fullfilename))
                return null;

            using (var fs = new FileStream(fullfilename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    using (var jr = new JsonTextReader(sr))
                    {
                        return _serializer.Deserialize(jr);
                    }
                }
            }
        }

        /// <summary>
        /// Json从文件反序列化
        /// </summary>
        /// <typeparam name="T">反序列化对象类型</typeparam>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string filename)
        {
            if (!File.Exists(filename))
                return default;

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    using (var jr = new JsonTextReader(sr))
                    {
                        return _serializer.Deserialize<T>(jr);
                    }
                }
            }
        }
    }
}
