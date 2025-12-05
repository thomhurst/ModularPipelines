using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "create-index")]
public record AwsClouddirectoryCreateIndexOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--ordered-indexed-attribute-list")] string[] OrderedIndexedAttributeList
) : AwsOptions
{
    [CliOption("--parent-reference")]
    public string? ParentReference { get; set; }

    [CliOption("--link-name")]
    public string? LinkName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}