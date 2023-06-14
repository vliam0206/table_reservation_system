using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums;

public class ReservationTime
{
    private static readonly TimeSpan StartTime = TimeSpan.Parse("09:00:00");
    private static readonly TimeSpan EndTime = TimeSpan.Parse("21:00:00");
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(30);
    public static List<string> TimeList
    {
        get
        {
            return GenerateTimeList(StartTime, EndTime, Interval);
        }
    }    
    private static List<string> GenerateTimeList(TimeSpan startTime, TimeSpan endTime, TimeSpan interval)
    {
        List<string> timeList = new List<string>();
        TimeSpan currentTime = startTime;

        while (currentTime <= endTime)
        {
            timeList.Add(currentTime.ToString());
            currentTime = currentTime.Add(interval);
        }

        return timeList;

    }
}
