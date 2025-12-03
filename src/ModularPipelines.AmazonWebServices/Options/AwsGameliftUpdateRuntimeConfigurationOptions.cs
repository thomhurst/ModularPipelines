using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "update-runtime-configuration")]
public record AwsGameliftUpdateRuntimeConfigurationOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--runtime-configuration")] string RuntimeConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}