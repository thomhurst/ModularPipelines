using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("footprint", "measurement-endpoint-condition", "list")]
public record AzFootprintMeasurementEndpointConditionListOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;