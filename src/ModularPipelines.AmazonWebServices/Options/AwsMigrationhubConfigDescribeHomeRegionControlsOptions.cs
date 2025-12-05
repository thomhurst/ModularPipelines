using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("migrationhub-config", "describe-home-region-controls")]
public record AwsMigrationhubConfigDescribeHomeRegionControlsOptions : AwsOptions
{
    [CliOption("--control-id")]
    public string? ControlId { get; set; }

    [CliOption("--home-region")]
    public string? HomeRegion { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}