using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "add-tags")]
public record AwsEmrAddTagsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--tags")] string Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}