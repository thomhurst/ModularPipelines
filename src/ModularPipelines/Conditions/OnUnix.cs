using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Conditions;

/// <summary>
/// A condition group that returns true when running on Linux or macOS.
/// </summary>
/// <remarks>
/// This is a convenience group that combines <see cref="OnLinux"/> and <see cref="OnMacOS"/>
/// with OR logic.
/// </remarks>
/// <example>
/// <code>
/// [RunIfAny&lt;OnUnix&gt;]
/// public class UnixModule : Module&lt;None&gt; { }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnUnix : ConditionGroup
{
    /// <inheritdoc />
    public override IRunCondition[] Conditions => [new OnLinux(), new OnMacOS()];

    /// <inheritdoc />
    public override ConditionLogic Logic => ConditionLogic.Any;
}
