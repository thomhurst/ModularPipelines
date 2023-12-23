using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "modify-instance-groups")]
public record AwsEmrModifyInstanceGroupsOptions : AwsOptions
{
    [CommandSwitch("--cluster-id")]
    public string? ClusterId { get; set; }

    [CommandSwitch("--instance-groups")]
    public string[]? InstanceGroups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}