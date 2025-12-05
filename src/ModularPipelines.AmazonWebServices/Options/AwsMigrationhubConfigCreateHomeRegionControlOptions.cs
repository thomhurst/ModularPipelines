using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhub-config", "create-home-region-control")]
public record AwsMigrationhubConfigCreateHomeRegionControlOptions(
[property: CliOption("--home-region")] string HomeRegion,
[property: CliOption("--target")] string Target
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}