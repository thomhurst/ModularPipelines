using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "get-config")]
public record AwsGroundstationGetConfigOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--config-type")] string ConfigType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}