using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.Principal;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsAdminRequirement : IPipelineRequirement
{
    public Task<bool> MustAsync(IPipelineContext context)
    {
        if (context.Environment.OperatingSystem == OSPlatform.Windows)
        {
#pragma warning disable CA1416
            return Task.FromResult(WindowsIdentity.GetCurrent().Owner?.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid) ?? true);
#pragma warning restore CA1416
        }

        return Task.FromResult(true);
    }
}
