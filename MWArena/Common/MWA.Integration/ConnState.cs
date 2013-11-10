using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWA.Integration
{
    public enum ConnState
    {
        ACTIVE,
        CONNECTED,
        CONNECTING,
        DISCONNECTING,
        DISCONNECTED,
        ERROR,
        INITIALIZING,
        NOTINITIALIZED,
        UNKNOWN
    }
}
