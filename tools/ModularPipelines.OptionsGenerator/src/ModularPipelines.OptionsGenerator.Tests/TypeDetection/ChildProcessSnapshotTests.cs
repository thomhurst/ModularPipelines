using System.Diagnostics;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ChildProcessSnapshotTests
{
    [Test]
    public async Task Snapshot_Finds_Direct_Child_Of_Current_Process()
    {
        if (!ChildProcessSnapshot.IsSupported)
        {
            return;
        }

        var startInfo = OperatingSystem.IsWindows()
            ? new ProcessStartInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),
                "WindowsPowerShell", "v1.0", "powershell.exe"), "-NoProfile -NonInteractive -Command Start-Sleep -Seconds 60")
            : new ProcessStartInfo("/bin/sleep", "60");
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        using var child = Process.Start(startInfo)!;
        try
        {
            var snapshot = ChildProcessSnapshot.Create();
            await Assert.That(snapshot.GetChildProcessIds(Environment.ProcessId)).Contains(child.Id);
            await Assert.That(snapshot.GetChildProcessIds(int.MaxValue)).IsEmpty();
        }
        finally
        {
            if (!child.HasExited)
            {
                child.Kill(entireProcessTree: true);
            }

            await child.WaitForExitAsync();
        }
    }
}
