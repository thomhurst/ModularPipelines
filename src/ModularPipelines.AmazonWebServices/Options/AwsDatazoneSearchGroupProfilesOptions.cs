using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datazone", "search-group-profiles")]
public record AwsDatazoneSearchGroupProfilesOptions(
[property: CliOption("--domain-identifier")] string DomainIdentifier,
[property: CliOption("--group-type")] string GroupType
) : AwsOptions
{
    [CliOption("--search-text")]
    public string? SearchText { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}