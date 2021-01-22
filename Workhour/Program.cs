using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Workhour
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventlog = new EventLog();
            eventlog.Log = "System";

            var times = new List<DateTime>();

            var dayOfWeek = (int)DateTime.Now.DayOfWeek;
            var diffDay = -1 * (dayOfWeek == 0 ? 6 : dayOfWeek - 1);

            if (args.Length > 0)
            {
                if (int.TryParse(args[0], out int backweek))
                {
                    diffDay += backweek * 7;
                }
            }

            var beginDate = DateTime.Today.AddDays(diffDay);
            var endDate = beginDate.AddDays(7).AddSeconds(-1);
            Console.WriteLine("Week start: {0:yyyy-MM-dd}", beginDate);
            Console.WriteLine();

            var powerons = new List<long>() { 18, 25, 27, 30, 32, 153 };
            var poweroffs = new List<long>() { 42, 107, 109, 187 };

            foreach (EventLogEntry entry in eventlog.Entries)
            {
                if (entry.TimeGenerated < beginDate)
                {
                    continue;
                }

                if (entry.TimeGenerated >= endDate)
                {
                    break;
                }

                // Boot
                if (powerons.Contains(entry.InstanceId) && times.Count % 2 == 0)
                {
                    times.Add(entry.TimeGenerated);
                    Console.WriteLine("+: {0:yyyy-MM-dd HH:mm}", entry.TimeGenerated);
                }

                // Poweroff
                if (poweroffs.Contains(entry.InstanceId) && times.Count % 2 == 1)
                {
                    times.Add(entry.TimeGenerated);
                    Console.WriteLine("-: {0:yyyy-MM-dd HH:mm}", entry.TimeGenerated);
                    Console.WriteLine("=: {0:0.##}", (times[times.Count - 1] - times[times.Count - 2]).TotalMinutes / 60);
                }
            }

            if (endDate >= DateTime.Now)
            {
                times.Add(DateTime.Now);
                Console.WriteLine("-: {0:yyyy-MM-dd HH:mm}", times[times.Count - 1]);
                Console.WriteLine("=: {0:0.##}", (times[times.Count - 1] - times[times.Count - 2]).TotalMinutes / 60);
            }

            Console.WriteLine();

            var totalMinutes = 0;
            for (int i = 0; i < times.Count / 2; i++)
            {
                totalMinutes += (int)(times[i * 2 + 1] - times[i * 2]).TotalMinutes;
            }
            var totalDays = (int)(Min(DateTime.Now, endDate) - beginDate).TotalDays + 1;
            Console.WriteLine("Work hours: {0:0.##}", totalMinutes / 60f - totalDays);
        }

        static DateTime Min(DateTime a, DateTime b)
        {
            return a <= b ? a : b;
        }
    }
}
