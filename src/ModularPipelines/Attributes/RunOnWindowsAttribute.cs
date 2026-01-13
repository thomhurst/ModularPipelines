using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module can run on Windows.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses OR logic with other <see cref="RunConditionAttribute"/> attributes.
/// When combined with other <c>RunOn*</c> attributes (e.g., <see cref="RunOnLinuxAttribute"/>),
/// the module will run if ANY of the conditions are met.
/// </para>
/// <para>
/// Use this attribute when you want to specify multiple platforms on which the module can execute.
/// For example, applying both <c>[RunOnWindows]</c> and <c>[RunOnLinux]</c> means the module
/// will run on either Windows OR Linux, but will be skipped on macOS.
/// </para>
/// <para>
/// <b>Example - Run on multiple platforms:</b>
/// <code>
/// [RunOnWindows]
/// [RunOnLinux]
/// public class CrossPlatformModule : Module&lt;None&gt;
/// {
///     // This module runs on Windows OR Linux, skipped on macOS
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnWindowsOnlyAttribute"/>
/// <seealso cref="RunOnLinuxAttribute"/>
/// <seealso cref="RunOnMacOSAttribute"/>
/// <seealso cref="RunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnWindowsAttribute : RunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}