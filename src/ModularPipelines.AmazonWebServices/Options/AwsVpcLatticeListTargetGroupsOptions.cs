using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "list-target-groups")]
public record AwsVpcLatticeListTargetGroupsOptions : AwsOptions
{
    [CommandSwitch("--target-group-type")]
    public string? TargetGroupType { get; set; }

    [CommandSwitch("--vpc-identifier")]
    public string? VpcIdentifier { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}