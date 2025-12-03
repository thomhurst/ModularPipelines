using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "update-config")]
public record AwsGroundstationUpdateConfigOptions(
[property: CliOption("--config-data")] string ConfigData,
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--config-type")] string ConfigType,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}