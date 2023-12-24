using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "retrieve")]
public record AwsKendraRetrieveOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--query-text")] string QueryText
) : AwsOptions
{
    [CommandSwitch("--attribute-filter")]
    public string? AttributeFilter { get; set; }

    [CommandSwitch("--requested-document-attributes")]
    public string[]? RequestedDocumentAttributes { get; set; }

    [CommandSwitch("--document-relevance-override-configurations")]
    public string[]? DocumentRelevanceOverrideConfigurations { get; set; }

    [CommandSwitch("--page-number")]
    public int? PageNumber { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--user-context")]
    public string? UserContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}