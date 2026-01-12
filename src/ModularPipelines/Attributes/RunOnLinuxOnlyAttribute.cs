using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module must run only on Linux.
/// The module will be skipped on all other operating systems.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses AND logic with other <see cref="MandatoryRunConditionAttribute"/> attributes.
/// ALL mandatory conditions must be satisfied for the module to run.
/// </para>
/// <para>
/// Use this attribute when your module contains Linux-specific logic that cannot
/// and should not run on other platforms.
/// </para>
/// <para>
/// <b>Difference from <see cref="RunOnLinuxAttribute"/>:</b>
/// <list type="bullet">
/// <item><description><c>[RunOnLinux]</c> - Uses OR logic; can be combined with other <c>RunOn*</c> attributes to run on multiple platforms.</description></item>
/// <item><description><c>[RunOnLinuxOnly]</c> - Uses AND logic; the module will ONLY run on Linux regardless of other attributes.</description></item>
/// </list>
/// </para>
/// <para>
/// <b>Example:</b>
/// <code>
/// [RunOnLinuxOnly]
/// public class SystemdServiceModule : Module&lt;None&gt;
/// {
///     // This module will only execute on Linux
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnLinuxAttribute"/>
/// <seealso cref="RunOnWindowsOnlyAttribute"/>
/// <seealso cref="RunOnMacOSOnlyAttribute"/>
/// <seealso cref="MandatoryRunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnLinuxOnlyAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsLinux());
    }
}