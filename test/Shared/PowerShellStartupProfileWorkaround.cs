namespace ModularPipelines.TestHelpers;

// Tests start many pwsh processes at once. Concurrent pwsh starts write the shared
// ~/.cache/powershell/StartupProfileData-* multicore JIT profile at the same time and can
// corrupt it, after which every pwsh start crashes (exit 134 FileLoadException, 139, or a
// stack overflow) until the file is deleted, in later runs too.
// https://github.com/PowerShell/PowerShell/issues/26528
// https://github.com/dotnet/runtime/issues/121977
// Requiring more CPUs than any machine has stops multicore JIT from starting in child
// processes, so they neither read nor write the profile. Child processes inherit this.
public static class PowerShellStartupProfileWorkaround
{
    [Before(TestDiscovery)]
    public static void DisableMultiCoreJitInChildProcesses()
    {
        Environment.SetEnvironmentVariable("DOTNET_MultiCoreJitMinNumCpus", "65535");
    }
}
