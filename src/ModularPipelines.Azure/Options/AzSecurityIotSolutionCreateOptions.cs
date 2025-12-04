using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "iot-solution", "create")]
public record AzSecurityIotSolutionCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--iot-hubs")] string IotHubs,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--solution-name")] string SolutionName
) : AzOptions;