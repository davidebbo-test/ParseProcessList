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
            if (args.Length != 2)
            {
                Console.WriteLine("Syntax: ParseProcessList file1 file2");
                return;
            }

            var first = ParseFile(args[0]);
            var second = ParseFile(args[1]);

            foreach(var newEntry in second.Values)
            {
                if (first.TryGetValue(newEntry.PID, out ProcessEntry oldEntry))
                {
                    newEntry.CpuTimeDelta = newEntry.CpuTime - oldEntry.CpuTime;
                }
            }

            foreach (var newEntry in second.Values.Where(e => e.CpuTimeDelta != TimeSpan.Zero).OrderByDescending(e => e.CpuTimeDelta))
            {
                Console.WriteLine($"{newEntry.PID,-12} {newEntry.Exe,-35} {newEntry.Process,-35}: {newEntry.CpuTimeDelta}");
            }
        }

        static IDictionary<string, ProcessEntry> ParseFile(string file)
        {
            var dict = new Dictionary<string, ProcessEntry>();

            foreach (var line in File.ReadAllLines(file))
            {
                ProcessEntry entry = ProcessEntry.Parse(line);
                if (entry == null) continue;

                dict[entry.PID] = entry;
            }

            return dict;
        }
    }
}
