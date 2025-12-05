using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "describe-endpoint-settings")]
public record AwsDmsDescribeEndpointSettingsOptions(
[property: CliOption("--engine-name")] string EngineName
) : AwsOptions
{
    [CliOption("--max-records")]
    public int? MaxRecords { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}