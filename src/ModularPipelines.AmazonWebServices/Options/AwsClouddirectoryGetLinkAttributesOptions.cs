using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "get-link-attributes")]
public record AwsClouddirectoryGetLinkAttributesOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--typed-link-specifier")] string TypedLinkSpecifier,
[property: CliOption("--attribute-names")] string[] AttributeNames
) : AwsOptions
{
    [CliOption("--consistency-level")]
    public string? ConsistencyLevel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}