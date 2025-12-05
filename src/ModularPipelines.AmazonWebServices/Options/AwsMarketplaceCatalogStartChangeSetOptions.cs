using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-catalog", "start-change-set")]
public record AwsMarketplaceCatalogStartChangeSetOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--change-set")] string[] ChangeSet
) : AwsOptions
{
    [CliOption("--change-set-name")]
    public string? ChangeSetName { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--change-set-tags")]
    public string[]? ChangeSetTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}