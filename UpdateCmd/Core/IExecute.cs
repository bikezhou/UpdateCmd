using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Core
{
    public interface IExecute<T>
    {
        int Execute(T options);
    }
}
