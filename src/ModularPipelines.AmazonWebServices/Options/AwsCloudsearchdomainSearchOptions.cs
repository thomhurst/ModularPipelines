using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudsearchdomain", "search")]
public record AwsCloudsearchdomainSearchOptions(
[property: CommandSwitch("--search-query")] string SearchQuery
) : AwsOptions
{
    [CommandSwitch("--cursor")]
    public string? Cursor { get; set; }

    [CommandSwitch("--expr")]
    public string? Expr { get; set; }

    [CommandSwitch("--facet")]
    public string? Facet { get; set; }

    [CommandSwitch("--filter-query")]
    public string? FilterQuery { get; set; }

    [CommandSwitch("--highlight")]
    public string? Highlight { get; set; }

    [CommandSwitch("--query-options")]
    public string? QueryOptions { get; set; }

    [CommandSwitch("--query-parser")]
    public string? QueryParser { get; set; }

    [CommandSwitch("--return")]
    public string? Return { get; set; }

    [CommandSwitch("--size")]
    public long? Size { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--start")]
    public long? Start { get; set; }

    [CommandSwitch("--stats")]
    public string? Stats { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}