using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "backend-pool", "delete")]
public record AzNetworkFrontDoorBackendPoolDeleteOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;