using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ParseProcessList
{
    class Program
    {
        static void Main(string[] args)
        {
            var first = ParseFile();
            var second = ParseFile();

            foreach(var newEntry in second.Values)
            {
                if (first.TryGetValue(newEntry.PID, out ProcessEntry oldEntry))
                {
                    newEntry.CpuTimeDelta = newEntry.CpuTime - oldEntry.CpuTime;
                }
            }

            foreach (var newEntry in second.Values.Where(e => e.CpuTimeDelta != TimeSpan.Zero).OrderByDescending(e => e.CpuTimeDelta).Take(10))
            {
                Console.WriteLine($"{newEntry.PID,-12} {newEntry.Exe,-35} {newEntry.Process,-35}: {newEntry.CpuTimeDelta}");
            }
        }

        static IDictionary<string, ProcessEntry> ParseFile()
        {
            var dict = new Dictionary<string, ProcessEntry>();

            for (; ; )
            {
                string line = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(line)) break;

                // Skip lines that don't start with a PID
                if (!Char.IsDigit(line[0])) continue;

                ProcessEntry entry = ProcessEntry.Parse(line);
                if (entry == null) continue;

                dict[entry.PID] = entry;
            }

            return dict;
        }
    }
}
