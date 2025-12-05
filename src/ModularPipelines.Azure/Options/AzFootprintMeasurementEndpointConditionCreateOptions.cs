using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("footprint", "measurement-endpoint-condition", "create")]
public record AzFootprintMeasurementEndpointConditionCreateOptions(
[property: CliOption("--constant")] string Constant,
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--name")] string Name,
[property: CliOption("--operator")] string Operator,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--variable")] string Variable
) : AzOptions;