using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog-appregistry", "disassociate-resource")]
public record AwsServicecatalogAppregistryDisassociateResourceOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--resource-type")] string ResourceType,
[property: CliOption("--resource")] string Resource
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}