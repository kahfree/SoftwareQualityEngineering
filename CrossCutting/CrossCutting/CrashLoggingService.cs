﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossCutting
{
    public class CrashLoggingService
    {
        public virtual void LogError(string message)
        {
            // Production code here which invokes crash logging Web 
            // service and passes the message and will throw 
            // an exception if there is a problem.
            // etc.
        }
    }
}
