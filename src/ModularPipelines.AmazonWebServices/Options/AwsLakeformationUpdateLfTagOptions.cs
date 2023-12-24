using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "update-lf-tag")]
public record AwsLakeformationUpdateLfTagOptions(
[property: CommandSwitch("--tag-key")] string TagKey
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--tag-values-to-delete")]
    public string[]? TagValuesToDelete { get; set; }

    [CommandSwitch("--tag-values-to-add")]
    public string[]? TagValuesToAdd { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}