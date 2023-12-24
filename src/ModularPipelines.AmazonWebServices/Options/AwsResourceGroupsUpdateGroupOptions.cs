using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "update-group")]
public record AwsResourceGroupsUpdateGroupOptions : AwsOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}