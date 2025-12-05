using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "search-listings")]
public record AwsDatazoneSearchListingsOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CliOption("--additional-attributes")]
    public string[]? AdditionalAttributes { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--search-in")]
    public string[]? SearchIn { get; set; }

    [CliOption("--search-text")]
    public string? SearchText { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}