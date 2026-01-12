using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module must run only on macOS.
/// The module will be skipped on all other operating systems.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses AND logic with other <see cref="MandatoryRunConditionAttribute"/> attributes.
/// ALL mandatory conditions must be satisfied for the module to run.
/// </para>
/// <para>
/// Use this attribute when your module contains macOS-specific logic that cannot
/// and should not run on other platforms.
/// </para>
/// <para>
/// <b>Difference from <see cref="RunOnMacOSAttribute"/>:</b>
/// <list type="bullet">
/// <item><description><c>[RunOnMacOS]</c> - Uses OR logic; can be combined with other <c>RunOn*</c> attributes to run on multiple platforms.</description></item>
/// <item><description><c>[RunOnMacOSOnly]</c> - Uses AND logic; the module will ONLY run on macOS regardless of other attributes.</description></item>
/// </list>
/// </para>
/// <para>
/// <b>Example:</b>
/// <code>
/// [RunOnMacOSOnly]
/// public class XcodeToolsModule : Module&lt;None&gt;
/// {
///     // This module will only execute on macOS
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnMacOSAttribute"/>
/// <seealso cref="RunOnWindowsOnlyAttribute"/>
/// <seealso cref="RunOnLinuxOnlyAttribute"/>
/// <seealso cref="MandatoryRunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnMacOSOnlyAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsMacOS());
    }
}