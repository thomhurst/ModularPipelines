using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog-appregistry", "associate-resource")]
public record AwsServicecatalogAppregistryAssociateResourceOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--options")]
    public string[]? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}