using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using UpdateCmd.Helpers;
using UpdateCmd.Properties;

namespace UpdateCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            using (var engine = new UpdateCmdEngine())
            {
                engine.Execute(args);
            }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name).Name;

            var dllname = $"{typeof(Program).Namespace}.Resources.{name}.dll";
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dllname))
            {
                if (stream == null)
                    return null;

                var data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                return Assembly.Load(data);
            }
        }
    }
}
