using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalD3D11.Console
{
    public class ConsoleCommand
    {
        public delegate void OnCommandEventHandler(ConsoleCommandArgs e);

        public string Command { get; set; }
        public OnCommandEventHandler Receiver { get; set; }
        public string Description { get; set; }
        public string Syntax { get; set; }

        public List<string> AutoCompleteCollection { get; set; }

        public ConsoleCommand()
        {
            AutoCompleteCollection = new List<string>();
        }

        public ConsoleCommand(string command, OnCommandEventHandler receiver, string description, string syntax)
        {
            AutoCompleteCollection = new List<string>();
            Command = command;
            Receiver = receiver;
            Description = description;
            Syntax = syntax;
        }
    }
}
