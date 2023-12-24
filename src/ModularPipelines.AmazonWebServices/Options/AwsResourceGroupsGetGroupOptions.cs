using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "get-group")]
public record AwsResourceGroupsGetGroupOptions : AwsOptions
{
    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}