using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "get-object-attributes")]
public record AwsClouddirectoryGetObjectAttributesOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference,
[property: CliOption("--schema-facet")] string SchemaFacet,
[property: CliOption("--attribute-names")] string[] AttributeNames
) : AwsOptions
{
    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}