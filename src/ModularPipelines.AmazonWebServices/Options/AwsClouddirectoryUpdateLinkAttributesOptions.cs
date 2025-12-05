using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "update-link-attributes")]
public record AwsClouddirectoryUpdateLinkAttributesOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--typed-link-specifier")] string TypedLinkSpecifier,
[property: CliOption("--attribute-updates")] string[] AttributeUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}