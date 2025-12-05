using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicecatalog-appregistry", "associate-attribute-group")]
public record AwsServicecatalogAppregistryAssociateAttributeGroupOptions(
[property: CliOption("--application")] string Application,
[property: CliOption("--attribute-group")] string AttributeGroup
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}