using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint-email", "get-dedicated-ips")]
public record AwsPinpointEmailGetDedicatedIpsOptions : AwsOptions
{
    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}