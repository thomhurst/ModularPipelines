using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "attach-object")]
public record AwsClouddirectoryAttachObjectOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--parent-reference")] string ParentReference,
[property: CliOption("--child-reference")] string ChildReference,
[property: CliOption("--link-name")] string LinkName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}