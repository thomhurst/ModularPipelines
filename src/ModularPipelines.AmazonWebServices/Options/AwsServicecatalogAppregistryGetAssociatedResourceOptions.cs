using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog-appregistry", "get-associated-resource")]
public record AwsServicecatalogAppregistryGetAssociatedResourceOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--resource-tag-status")]
    public string[]? ResourceTagStatus { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}