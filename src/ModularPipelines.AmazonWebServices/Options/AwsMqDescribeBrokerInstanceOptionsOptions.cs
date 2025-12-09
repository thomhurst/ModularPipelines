using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "describe-broker-instance-options")]
public record AwsMqDescribeBrokerInstanceOptionsOptions : AwsOptions
{
    [CliOption("--engine-type")]
    public string? EngineType { get; set; }

    [CliOption("--host-instance-type")]
    public string? HostInstanceType { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}