using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using ModularPipelines.Context;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsAdminRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<bool> MustAsync(IPipelineHookContext context)
    {
        if (context.Environment.OperatingSystem == OperatingSystemIdentifier.Windows)
        {
#pragma warning disable CA1416
            return Task.FromResult(WindowsIdentity.GetCurrent().Owner?.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid) ?? true);
#pragma warning restore CA1416
        }

        return Task.FromResult(true);
    }
}
