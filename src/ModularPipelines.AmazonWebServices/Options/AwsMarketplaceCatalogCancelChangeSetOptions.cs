using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-catalog", "cancel-change-set")]
public record AwsMarketplaceCatalogCancelChangeSetOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--change-set-id")] string ChangeSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}