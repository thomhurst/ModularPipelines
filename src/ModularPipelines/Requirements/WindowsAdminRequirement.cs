using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

[ExcludeFromCodeCoverage]
public class WindowsAdminRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        if (context.Environment.OperatingSystem == OperatingSystemIdentifier.Windows)
        {
#pragma warning disable CA1416
            return RequirementDecision.Of(
                passed: WindowsIdentity.GetCurrent().Owner?.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid)
                        ?? true,
                reason: "Windows Admin is required."
            ).AsTask();
#pragma warning restore CA1416
        }

        return RequirementDecision.Passed.AsTask();
    }
}