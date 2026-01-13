using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module can run on macOS.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses OR logic with other <see cref="RunConditionAttribute"/> attributes.
/// When combined with other <c>RunOn*</c> attributes (e.g., <see cref="RunOnLinuxAttribute"/>),
/// the module will run if ANY of the conditions are met.
/// </para>
/// <para>
/// Use this attribute when you want to specify multiple platforms on which the module can execute.
/// For example, applying both <c>[RunOnMacOS]</c> and <c>[RunOnLinux]</c> means the module
/// will run on either macOS OR Linux, but will be skipped on Windows.
/// </para>
/// <para>
/// <b>Example - Run on multiple platforms:</b>
/// <code>
/// [RunOnMacOS]
/// [RunOnLinux]
/// public class UnixLikeModule : Module&lt;None&gt;
/// {
///     // This module runs on macOS OR Linux, skipped on Windows
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnMacOSOnlyAttribute"/>
/// <seealso cref="RunOnWindowsAttribute"/>
/// <seealso cref="RunOnLinuxAttribute"/>
/// <seealso cref="RunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnMacOSAttribute : RunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsMacOS());
    }
}