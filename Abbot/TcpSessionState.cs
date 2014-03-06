using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class TcpSessionState
    {
        public IEnumerator Line;
        public TcpListener Connection;
    }
}
