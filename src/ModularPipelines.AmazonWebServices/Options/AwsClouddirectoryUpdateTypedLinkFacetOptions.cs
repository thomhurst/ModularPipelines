using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "update-typed-link-facet")]
public record AwsClouddirectoryUpdateTypedLinkFacetOptions(
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--attribute-updates")] string[] AttributeUpdates,
[property: CliOption("--identity-attribute-order")] string[] IdentityAttributeOrder
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}