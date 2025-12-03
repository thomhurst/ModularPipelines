using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "search-cases")]
public record AwsConnectcasesSearchCasesOptions(
[property: CliOption("--domain-id")] string DomainId
) : AwsOptions
{
    [CliOption("--fields")]
    public string[]? Fields { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--search-term")]
    public string? SearchTerm { get; set; }

    [CliOption("--sorts")]
    public string[]? Sorts { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}