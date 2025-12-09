using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "attach-typed-link")]
public record AwsClouddirectoryAttachTypedLinkOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--source-object-reference")] string SourceObjectReference,
[property: CliOption("--target-object-reference")] string TargetObjectReference,
[property: CliOption("--typed-link-facet")] string TypedLinkFacet,
[property: CliOption("--attributes")] string[] Attributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}