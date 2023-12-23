using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "add-lf-tags-to-resource")]
public record AwsLakeformationAddLfTagsToResourceOptions(
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--lf-tags")] string[] LfTags
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}