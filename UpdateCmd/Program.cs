using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UpdateCmd.Helpers;

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
            var asm = Assembly.GetExecutingAssembly();

            var dllname = $"UpdateCmd.Resources.{name}.dll";
            using (var stream = asm.GetManifestResourceStream(dllname))
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
