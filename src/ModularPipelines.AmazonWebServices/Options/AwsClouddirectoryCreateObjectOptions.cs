using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "create-object")]
public record AwsClouddirectoryCreateObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--schema-facets")] string[] SchemaFacets
) : AwsOptions
{
    [CliOption("--object-attribute-list")]
    public string[]? ObjectAttributeList { get; set; }

    [CliOption("--parent-reference")]
    public string? ParentReference { get; set; }

    [CliOption("--link-name")]
    public string? LinkName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}