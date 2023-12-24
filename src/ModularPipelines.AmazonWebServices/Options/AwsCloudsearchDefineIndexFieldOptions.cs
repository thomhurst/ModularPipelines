using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearch", "define-index-field")]
public record AwsCloudsearchDefineIndexFieldOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--default-value")]
    public string? DefaultValue { get; set; }

    [BooleanCommandSwitch("--facet-enabled")]
    public bool? FacetEnabled { get; set; }

    [BooleanCommandSwitch("--search-enabled")]
    public bool? SearchEnabled { get; set; }

    [BooleanCommandSwitch("--return-enabled")]
    public bool? ReturnEnabled { get; set; }

    [BooleanCommandSwitch("--sort-enabled")]
    public bool? SortEnabled { get; set; }

    [CommandSwitch("--source-field")]
    public string? SourceField { get; set; }

    [BooleanCommandSwitch("--highlight-enabled")]
    public bool? HighlightEnabled { get; set; }

    [CommandSwitch("--analysis-scheme")]
    public string? AnalysisScheme { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}