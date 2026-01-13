using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that this module must run only on Windows.
/// The module will be skipped on all other operating systems.
/// </summary>
/// <remarks>
/// <para>
/// This attribute uses AND logic with other <see cref="MandatoryRunConditionAttribute"/> attributes.
/// ALL mandatory conditions must be satisfied for the module to run.
/// </para>
/// <para>
/// Use this attribute when your module contains Windows-specific logic that cannot
/// and should not run on other platforms.
/// </para>
/// <para>
/// <b>Difference from <see cref="RunOnWindowsAttribute"/>:</b>
/// <list type="bullet">
/// <item><description><c>[RunOnWindows]</c> - Uses OR logic; can be combined with other <c>RunOn*</c> attributes to run on multiple platforms.</description></item>
/// <item><description><c>[RunOnWindowsOnly]</c> - Uses AND logic; the module will ONLY run on Windows regardless of other attributes.</description></item>
/// </list>
/// </para>
/// <para>
/// <b>Example:</b>
/// <code>
/// [RunOnWindowsOnly]
/// public class WindowsRegistryModule : Module&lt;None&gt;
/// {
///     // This module will only execute on Windows
/// }
/// </code>
/// </para>
/// </remarks>
/// <seealso cref="RunOnWindowsAttribute"/>
/// <seealso cref="RunOnLinuxOnlyAttribute"/>
/// <seealso cref="RunOnMacOSOnlyAttribute"/>
/// <seealso cref="MandatoryRunConditionAttribute"/>
[ExcludeFromCodeCoverage]
public class RunOnWindowsOnlyAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(OperatingSystem.IsWindows());
    }
}