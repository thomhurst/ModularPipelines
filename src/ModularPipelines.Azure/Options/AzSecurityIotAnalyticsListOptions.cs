using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "iot-analytics", "list")]
public record AzSecurityIotAnalyticsListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions;