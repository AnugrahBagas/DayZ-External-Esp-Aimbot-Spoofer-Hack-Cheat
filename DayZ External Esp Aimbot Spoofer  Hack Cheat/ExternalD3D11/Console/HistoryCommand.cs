using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.Console
{
    public class HistoryCommand
    {
        public DateTime AddTime { get; set; }
        public string Command { get; set; }

        public HistoryCommand(){}

        public HistoryCommand(DateTime dt, string command)
        {
            AddTime = dt;
            Command = command;
        }
    }
}
