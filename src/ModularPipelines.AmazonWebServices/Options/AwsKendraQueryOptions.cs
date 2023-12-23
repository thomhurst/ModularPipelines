using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "query")]
public record AwsKendraQueryOptions(
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--query-text")]
    public string? QueryText { get; set; }

    [CommandSwitch("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CommandSwitch("--facets")]
    public string[]? Facets { get; set; }

    [CommandSwitch("--requested-document-attributes")]
    public string[]? RequestedDocumentAttributes { get; set; }

    [CommandSwitch("--query-result-type-filter")]
    public string? QueryResultTypeFilter { get; set; }

    [CommandSwitch("--document-relevance-override-configurations")]
    public string[]? DocumentRelevanceOverrideConfigurations { get; set; }

    [CommandSwitch("--page-number")]
    public int? PageNumber { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--sorting-configuration")]
    public string? SortingConfiguration { get; set; }

    [CommandSwitch("--sorting-configurations")]
    public string[]? SortingConfigurations { get; set; }

    [CommandSwitch("--user-context")]
    public string? UserContext { get; set; }

    [CommandSwitch("--visitor-id")]
    public string? VisitorId { get; set; }

    [CommandSwitch("--spell-correction-configuration")]
    public string? SpellCorrectionConfiguration { get; set; }

    [CommandSwitch("--collapse-configuration")]
    public string? CollapseConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}