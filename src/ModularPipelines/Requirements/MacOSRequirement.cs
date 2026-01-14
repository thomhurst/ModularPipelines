using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// A pipeline requirement that ensures the current operating system is macOS.
/// </summary>
/// <remarks>
/// <para>
/// Use this requirement when your pipeline requires macOS-specific functionality,
/// such as using Xcode, macOS system APIs, or Apple-specific tools.
/// </para>
/// <para><b>Example:</b></para>
/// <code>
/// await PipelineHostBuilder.Create()
///     .AddRequirement&lt;MacOSRequirement&gt;()
///     .AddModule&lt;BuildMacAppModule&gt;()
///     .ExecutePipelineAsync();
/// </code>
/// </remarks>
/// <seealso cref="WindowsRequirement"/>
/// <seealso cref="LinuxRequirement"/>
[ExcludeFromCodeCoverage]
public class MacOSRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineHookContext context)
    {
        return RequirementDecision.Of(
            passed: context.Environment.OperatingSystem == System.Runtime.InteropServices.OSPlatform.OSX,
            reason: "MacOS is required"
        ).AsTask();
    }
}