using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "create-featured-results-set")]
public record AwsKendraCreateFeaturedResultsSetOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--featured-results-set-name")] string FeaturedResultsSetName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--query-texts")]
    public string[]? QueryTexts { get; set; }

    [CommandSwitch("--featured-documents")]
    public string[]? FeaturedDocuments { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}