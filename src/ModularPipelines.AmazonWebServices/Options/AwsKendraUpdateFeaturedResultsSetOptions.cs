using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "update-featured-results-set")]
public record AwsKendraUpdateFeaturedResultsSetOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--featured-results-set-id")] string FeaturedResultsSetId
) : AwsOptions
{
    [CommandSwitch("--featured-results-set-name")]
    public string? FeaturedResultsSetName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--query-texts")]
    public string[]? QueryTexts { get; set; }

    [CommandSwitch("--featured-documents")]
    public string[]? FeaturedDocuments { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}