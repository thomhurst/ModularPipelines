using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-verified-access-endpoints")]
public record AwsEc2DescribeVerifiedAccessEndpointsOptions : AwsOptions
{
    [CommandSwitch("--verified-access-endpoint-ids")]
    public string[]? VerifiedAccessEndpointIds { get; set; }

    [CommandSwitch("--verified-access-instance-id")]
    public string? VerifiedAccessInstanceId { get; set; }

    [CommandSwitch("--verified-access-group-id")]
    public string? VerifiedAccessGroupId { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}