using System.Diagnostics;
using System.Runtime.InteropServices;

// Same loop as the upstream repro, plus a heartbeat file the out-of-tree watchdog monitors.
// SIGHUP is logged and cancelled, to test whether the kernel's orphaned-process-group rule
// (SIGHUP + SIGCONT to every member of a group that becomes orphaned with stopped members) is what kills the job.
var heartbeat = args[0];
void Log(string line) => File.AppendAllText(heartbeat, $"{DateTime.UtcNow:HH:mm:ss.fff} {line}\n");
File.WriteAllText(heartbeat, $"pid {Environment.ProcessId}\n");
using var hup = PosixSignalRegistration.Create(PosixSignal.SIGHUP, context => { Log("received SIGHUP (cancelled)"); context.Cancel = true; });
using var cont = PosixSignalRegistration.Create(PosixSignal.SIGCONT, _ => Log("received SIGCONT"));
for (var i = 1; i <= 200; i++)
{
    using var p = Process.Start(new ProcessStartInfo("/bin/sh", ["-c", "sleep 60 & sleep 60; wait"]) { RedirectStandardOutput = true })!;
    Thread.Sleep(300);
    Log($"iteration {i}: killing tree of {p.Id}");
    p.Kill(entireProcessTree: true);
    Log($"iteration {i}: kill returned");
    p.WaitForExit();
}
Log("completed 200 iterations");
