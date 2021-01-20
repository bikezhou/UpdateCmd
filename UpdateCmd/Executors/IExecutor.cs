using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UpdateCmd.Executors
{
    public interface IExecutor<T>
    {
        void Execute(T options);
    }
}
