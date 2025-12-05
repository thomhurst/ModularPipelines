using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhub-config", "delete-home-region-control")]
public record AwsMigrationhubConfigDeleteHomeRegionControlOptions(
[property: CliOption("--control-id")] string ControlId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}