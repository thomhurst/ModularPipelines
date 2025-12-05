using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "detach-object")]
public record AwsClouddirectoryDetachObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--parent-reference")] string ParentReference,
[property: CliOption("--link-name")] string LinkName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}