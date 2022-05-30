using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DayZMultiHack
{
    public static class NonBlockingTextWrite
    {
        private static BlockingCollection<string> m_Queue = new BlockingCollection<string>();

        static NonBlockingTextWrite()
        {
            if (!File.Exists("items.txt"))
                File.Create("items.txt");

            var thread = new Thread(
              () =>
              {
                  while (true)
                  {
                      File.AppendAllText("items.txt", m_Queue.Take());
                  }
              });
            thread.IsBackground = true;
            thread.Start();
        }

        public static void WriteLine(string value)
        {
            m_Queue.Add(value);
        }
    }
}
