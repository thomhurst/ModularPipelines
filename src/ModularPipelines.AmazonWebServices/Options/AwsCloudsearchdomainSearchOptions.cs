using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudsearchdomain", "search")]
public record AwsCloudsearchdomainSearchOptions(
[property: CliOption("--search-query")] string SearchQuery
) : AwsOptions
{
    [CliOption("--cursor")]
    public string? Cursor { get; set; }

    [CliOption("--expr")]
    public string? Expr { get; set; }

    [CliOption("--facet")]
    public string? Facet { get; set; }

    [CliOption("--filter-query")]
    public string? FilterQuery { get; set; }

    [CliOption("--highlight")]
    public string? Highlight { get; set; }

    [CliOption("--query-options")]
    public string? QueryOptions { get; set; }

    [CliOption("--query-parser")]
    public string? QueryParser { get; set; }

    [CliOption("--return")]
    public string? Return { get; set; }

    [CliOption("--size")]
    public long? Size { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--start")]
    public long? Start { get; set; }

    [CliOption("--stats")]
    public string? Stats { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}