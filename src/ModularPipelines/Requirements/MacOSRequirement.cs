using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
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
/// var builder = Pipeline.CreateBuilder();
/// builder.Services.AddRequirement&lt;MacOSRequirement&gt;();
/// builder.AddModule&lt;BuildMacAppModule&gt;();
///
/// await builder.RunAsync();
/// </code>
/// </remarks>
/// <seealso cref="WindowsRequirement"/>
/// <seealso cref="LinuxRequirement"/>
[ExcludeFromCodeCoverage]
public class MacOSRequirement : IPipelineRequirement
{
    /// <inheritdoc/>
    public Task<RequirementDecision> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(RequirementDecision.Of(
            passed: context.Environment.OperatingSystem == System.Runtime.InteropServices.OSPlatform.OSX,
            reason: "MacOS is required"
        ));
    }
}
