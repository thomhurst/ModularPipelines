using System.Diagnostics;

// Same loop as the upstream repro, plus a heartbeat file the out-of-tree watchdog monitors.
var heartbeat = args[0];
File.WriteAllText(heartbeat, $"pid {Environment.ProcessId}\n");
for (var i = 1; i <= 200; i++)
{
    using var p = Process.Start(new ProcessStartInfo("/bin/sh", ["-c", "sleep 60 & sleep 60; wait"]) { RedirectStandardOutput = true })!;
    Thread.Sleep(300);
    File.AppendAllText(heartbeat, $"{DateTime.UtcNow:HH:mm:ss.fff} iteration {i}: killing tree of {p.Id}\n");
    p.Kill(entireProcessTree: true);
    File.AppendAllText(heartbeat, $"{DateTime.UtcNow:HH:mm:ss.fff} iteration {i}: kill returned\n");
    p.WaitForExit();
}
File.AppendAllText(heartbeat, "completed 200 iterations\n");
