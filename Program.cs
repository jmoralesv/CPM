using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        List<Activity> activities = GetSampleActivities();

        Dictionary<string, int> earliestStartTimes = CalculateEarliestStartTimes(activities);
        Dictionary<string, int> latestStartTimes = CalculateLatestStartTimes(activities, earliestStartTimes);

        PrintActivityTable(activities, earliestStartTimes, latestStartTimes);

        int projectDuration = CalculateProjectDuration(activities, earliestStartTimes);
        Console.WriteLine($"Project Duration: {projectDuration}");

        Console.ReadKey();
    }

    static int CalculateProjectDuration(List<Activity> activities, Dictionary<string, int> earliestStartTimes)
    {
        // Find the maximum earliest start time among all end nodes
        int maxEndNodeTime = earliestStartTimes.Values.Max();

        // Find the minimum earliest start time among all start nodes
        int minStartNodeTime = earliestStartTimes.Values.Min();

        // Calculate the project duration
        return maxEndNodeTime - minStartNodeTime;
    }


    static void PrintActivityTable(List<Activity> activities, Dictionary<string, int> earliestStartTimes, Dictionary<string, int> latestStartTimes)
    {
        Console.WriteLine("Activity From-To\tDuration\tEarliest Start Time\tLatest Start Time");
        foreach (var activity in activities)
        {
            Console.WriteLine($"{activity.From}-{activity.To}\t\t{activity.Duration}\t\t{earliestStartTimes[activity.From]}\t\t\t{latestStartTimes[activity.From]}");
        }
    }

    static List<Activity> GetSampleActivities()
    {
        return new List<Activity>
        {
            new Activity("V1", "V2", 6),
            new Activity("V1", "V3", 4),
            new Activity("V1", "V4", 5),
            new Activity("V2", "V5", 1),
            new Activity("V3", "V5", 1),
            new Activity("V4", "V6", 2),
            new Activity("V5", "V7", 9),
            new Activity("V5", "V8", 7),
            new Activity("V6", "V8", 4),
            new Activity("V7", "V9", 2),
            new Activity("V8", "V9", 4),
        };
    }

    static Dictionary<string, int> CalculateEarliestStartTimes(List<Activity> activities)
    {
        Dictionary<string, int> earliestStartTimes = new Dictionary<string, int>();

        var startNodes = activities.Select(a => a.From).Except(activities.Select(a => a.To)).ToList();

        foreach (var startNode in startNodes)
        {
            earliestStartTimes[startNode] = 1;
            UpdateEarliestStartTimes(startNode, activities, earliestStartTimes);
        }

        return earliestStartTimes;
    }

    static void UpdateEarliestStartTimes(string node, List<Activity> activities, Dictionary<string, int> earliestStartTimes)
    {
        var outgoingEdges = activities.Where(a => a.From == node).ToList();

        foreach (var edge in outgoingEdges)
        {
            var newStartTime = earliestStartTimes[node] + edge.Duration;

            if (!earliestStartTimes.ContainsKey(edge.To) || earliestStartTimes[edge.To] < newStartTime)
            {
                earliestStartTimes[edge.To] = newStartTime;
                UpdateEarliestStartTimes(edge.To, activities, earliestStartTimes);
            }
        }
    }

    static Dictionary<string, int> CalculateLatestStartTimes(List<Activity> activities, Dictionary<string, int> earliestStartTimes)
    {
        Dictionary<string, int> latestStartTimes = new Dictionary<string, int>();

        foreach (var activity in activities)
        {
            var latestStartTime = earliestStartTimes[activity.To] - activity.Duration;
            if (!latestStartTimes.ContainsKey(activity.From) || latestStartTimes[activity.From] > latestStartTime)
            {
                latestStartTimes[activity.From] = latestStartTime;
            }
        }

        return latestStartTimes;
    }
}

class Activity
{
    public string From { get; }
    public string To { get; }
    public int Duration { get; }

    public Activity(string from, string to, int duration)
    {
        From = from;
        To = to;
        Duration = duration;
    }
}
