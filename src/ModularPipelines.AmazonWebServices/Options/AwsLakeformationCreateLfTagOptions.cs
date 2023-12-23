using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lakeformation", "create-lf-tag")]
public record AwsLakeformationCreateLfTagOptions(
[property: CommandSwitch("--tag-key")] string TagKey,
[property: CommandSwitch("--tag-values")] string[] TagValues
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}