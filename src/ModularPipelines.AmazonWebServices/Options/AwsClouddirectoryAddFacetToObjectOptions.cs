using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "add-facet-to-object")]
public record AwsClouddirectoryAddFacetToObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--schema-facet")] string SchemaFacet,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--object-attribute-list")]
    public string[]? ObjectAttributeList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}