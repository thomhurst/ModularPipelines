using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "automation-rule", "create")]
public record AzSecurityAutomationRuleCreateOptions(
[property: CliOption("--expected-value")] string ExpectedValue,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--property-j-path")] string PropertyJPath,
[property: CliOption("--property-type")] string PropertyType
) : AzOptions;