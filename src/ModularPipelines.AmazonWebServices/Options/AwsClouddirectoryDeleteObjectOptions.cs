using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "delete-object")]
public record AwsClouddirectoryDeleteObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}