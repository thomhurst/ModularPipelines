using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "attach-to-index")]
public record AwsClouddirectoryAttachToIndexOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--index-reference")] string IndexReference,
[property: CliOption("--target-reference")] string TargetReference
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}