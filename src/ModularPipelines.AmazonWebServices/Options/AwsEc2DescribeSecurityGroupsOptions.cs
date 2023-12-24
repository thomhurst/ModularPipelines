using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-security-groups")]
public record AwsEc2DescribeSecurityGroupsOptions : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--group-ids")]
    public string[]? GroupIds { get; set; }

    [CommandSwitch("--group-names")]
    public string[]? GroupNames { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}