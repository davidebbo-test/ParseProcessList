using System;
using System.Collections.Generic;
using System.Text;

namespace ParseProcessList
{
    class ProcessEntry
    {
        public static ProcessEntry Parse(string line)
        {
            if (line.StartsWith("Pid")) return null;

            string[] parts = line.Split(new char[] { ' ' }, options: StringSplitOptions.RemoveEmptyEntries);

            var entry = new ProcessEntry();

            entry.PID = parts[0];
            entry.WorkingSet = Int32.Parse(parts[1]);
            entry.HandleCount = Int32.Parse(parts[3]);
            var timeParts = parts[4].Split(':');
            entry.CpuTime = new TimeSpan(0, Int32.Parse(timeParts[0]), Int32.Parse(timeParts[1]), Int32.Parse(timeParts[2]));
            entry.Exe = parts[5];
            if (parts.Length > 6)
            {
                entry.Process = parts[6];
            }

            return entry;
        }

        public string PID { get; set; }
        public int WorkingSet { get; set; }
        public int HandleCount { get; set; }
        public TimeSpan CpuTime { get; set; }
        public TimeSpan CpuTimeDelta { get; set; }
        public string Exe { get; set; }
        public string Process { get; set; }
    }
}
