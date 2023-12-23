using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "get-group-version")]
public record AwsGreengrassGetGroupVersionOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--group-version-id")] string GroupVersionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}