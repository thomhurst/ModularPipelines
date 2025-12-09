using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "update-object-attributes")]
public record AwsClouddirectoryUpdateObjectAttributesOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference,
[property: CliOption("--attribute-updates")] string[] AttributeUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}