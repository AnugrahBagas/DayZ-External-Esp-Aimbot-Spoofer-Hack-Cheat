using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.Console
{
    public class ConsoleCommandArgs
    {
        public string[] CommandData { get; set; }
        public ConsoleCommand Command { get; set; }
        public D3DConsole.LogCallbackEventHandler Callback { get; set; }

        public ConsoleCommandArgs(){}

        public ConsoleCommandArgs(string[] commandData, ConsoleCommand command, D3DConsole.LogCallbackEventHandler callback)
        {
            CommandData = commandData;
            Command = command;
            Callback = callback;
        }
    }
}
