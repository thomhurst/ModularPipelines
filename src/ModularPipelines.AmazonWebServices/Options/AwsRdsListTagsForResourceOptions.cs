using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "list-tags-for-resource")]
public record AwsRdsListTagsForResourceOptions(
[property: CommandSwitch("--resource-name")] string ResourceName
) : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}