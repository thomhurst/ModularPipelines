using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "add-tags")]
public record AwsEsAddTagsOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--tag-list")] string[] TagList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}