using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "automation-rule", "create")]
public record AzSecurityAutomationRuleCreateOptions(
[property: CommandSwitch("--expected-value")] string ExpectedValue,
[property: CommandSwitch("--operator")] string Operator,
[property: CommandSwitch("--property-j-path")] string PropertyJPath,
[property: CommandSwitch("--property-type")] string PropertyType
) : AzOptions;