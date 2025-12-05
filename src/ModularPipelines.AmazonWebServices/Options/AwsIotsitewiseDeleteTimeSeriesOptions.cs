using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "delete-time-series")]
public record AwsIotsitewiseDeleteTimeSeriesOptions : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--asset-id")]
    public string? AssetId { get; set; }

    [CliOption("--property-id")]
    public string? PropertyId { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}