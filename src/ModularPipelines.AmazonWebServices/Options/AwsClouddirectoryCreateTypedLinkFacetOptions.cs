using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "create-typed-link-facet")]
public record AwsClouddirectoryCreateTypedLinkFacetOptions(
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--facet")] string Facet
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}