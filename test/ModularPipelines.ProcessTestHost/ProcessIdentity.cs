using System.Diagnostics;
using System.Globalization;

namespace ModularPipelines.ProcessTestHost;

public readonly record struct ProcessIdentity(int Id, long StartTimeValue)
{
    public static ProcessIdentity Capture(Process process)
    {
        if (process.HasExited)
        {
            throw new InvalidOperationException("Cannot capture the identity of an exited process.");
        }

        // Linux Process.StartTime converts kernel ticks using a boot-time estimate
        // cached separately in each observer. Compare the kernel value directly.
        var startTime = OperatingSystem.IsLinux()
            ? ParseLinuxStartTime(File.ReadAllText($"/proc/{process.Id}/stat"))
            : process.StartTime.ToUniversalTime().Ticks;
        if (process.HasExited)
        {
            throw new InvalidOperationException("Process exited while its identity was being captured.");
        }

        return new(process.Id, startTime);
    }

    public static long ParseLinuxStartTime(string stat)
    {
        // comm (field 2) can contain spaces and parentheses. Fields after its final
        // closing parenthesis start at state (field 3); starttime is field 22.
        var fields = stat[(stat.LastIndexOf(')') + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return long.Parse(fields[19], CultureInfo.InvariantCulture);
    }
}
