using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the process is running with Windows Administrator privileges.
/// </summary>
/// <remarks>
/// <para>
/// Use this requirement when your pipeline needs elevated privileges on Windows,
/// such as modifying system settings, installing software to protected directories,
/// or managing Windows services.
/// </para>
/// <para>
/// On non-Windows platforms, this requirement always passes.
/// </para>
/// <para><b>Example:</b></para>
/// <code>
/// await PipelineHostBuilder.Create()
///     .AddRequirement&lt;WindowsAdminRequirement&gt;()
///     .AddModule&lt;InstallServiceModule&gt;()
///     .ExecutePipelineAsync();
/// </code>
/// </remarks>
/// <seealso cref="WindowsRequirement"/>
[ExcludeFromCodeCoverage]
public class WindowsAdminRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        if (context.Environment.OperatingSystem == System.Runtime.InteropServices.OSPlatform.Windows)
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