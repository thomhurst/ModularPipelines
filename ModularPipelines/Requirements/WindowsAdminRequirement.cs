using System.Security.Principal;

namespace ModularPipelines.Requirements;

public class WindowsAdminRequirement : IPipelineRequirement
{
    public Task<bool> MustAsync()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        {
#pragma warning disable CA1416
            return Task.FromResult(WindowsIdentity.GetCurrent().Owner?.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid) ?? true);
#pragma warning restore CA1416
        }

        return Task.FromResult(true);
    }
}