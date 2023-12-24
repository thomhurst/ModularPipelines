using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-placement-groups")]
public record AwsEc2DescribePlacementGroupsOptions : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--group-names")]
    public string[]? GroupNames { get; set; }

    [CommandSwitch("--group-ids")]
    public string[]? GroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}