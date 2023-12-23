using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-groups", "get-group-configuration")]
public record AwsResourceGroupsGetGroupConfigurationOptions : AwsOptions
{
    [CommandSwitch("--group")]
    public string? Group { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}