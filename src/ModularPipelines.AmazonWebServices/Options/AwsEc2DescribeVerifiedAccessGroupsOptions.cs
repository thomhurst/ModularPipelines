using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-verified-access-groups")]
public record AwsEc2DescribeVerifiedAccessGroupsOptions : AwsOptions
{
    [CommandSwitch("--verified-access-group-ids")]
    public string[]? VerifiedAccessGroupIds { get; set; }

    [CommandSwitch("--verified-access-instance-id")]
    public string? VerifiedAccessInstanceId { get; set; }

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