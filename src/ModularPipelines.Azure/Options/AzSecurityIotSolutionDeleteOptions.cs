using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "iot-solution", "delete")]
public record AzSecurityIotSolutionDeleteOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions;