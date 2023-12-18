using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("security", "iot-solution", "create")]
public record AzSecurityIotSolutionCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--iot-hubs")] string IotHubs,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--solution-name")] string SolutionName
) : AzOptions;