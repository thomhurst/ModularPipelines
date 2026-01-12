using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module can run on Linux.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses OR logic with other <see cref="RunConditionAttribute"/> attributes.
/// When combined with other <c>RunOn*</c> attributes (e.g., <see cref="RunOnWindowsAttribute"/>),
/// the module will run if ANY of the conditions are met.
/// </para>
/// <para>
/// Use this attribute when you want to specify multiple platforms on which the module can execute.
/// For example, applying both <c>[RunOnLinux]</c> and <c>[RunOnMacOS]</c> means the module
/// will run on either Linux OR macOS, but will be skipped on Windows.
/// </para>
/// <para>
/// <b>Example - Run on multiple platforms:</b>
/// <code>
/// [RunOnLinux]
/// [RunOnMacOS]
/// public class UnixLikeModule : Module&lt;None&gt;
/// {
///     // This module runs on Linux OR macOS, skipped on Windows
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnLinuxOnlyAttribute"/>
/// <seealso cref="RunOnWindowsAttribute"/>
/// <seealso cref="RunOnMacOSAttribute"/>
/// <seealso cref="RunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnLinuxAttribute : RunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsLinux());
    }
}