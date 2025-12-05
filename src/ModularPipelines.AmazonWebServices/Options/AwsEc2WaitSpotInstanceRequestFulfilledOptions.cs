using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "spot-instance-request-fulfilled")]
public record AwsEc2WaitSpotInstanceRequestFulfilledOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--spot-instance-request-ids")]
    public string[]? SpotInstanceRequestIds { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}