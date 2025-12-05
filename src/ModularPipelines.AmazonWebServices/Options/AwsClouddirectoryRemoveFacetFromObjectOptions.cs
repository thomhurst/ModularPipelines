using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "remove-facet-from-object")]
public record AwsClouddirectoryRemoveFacetFromObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--schema-facet")] string SchemaFacet,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}