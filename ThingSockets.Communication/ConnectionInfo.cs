﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThingSockets.External;

namespace ThingSockets.Communication
{
    sealed class ConnectionInfo : IListenerAddress
    {
        public string Ip { get; set; }

        public string Port { get; set; }
    }
}
