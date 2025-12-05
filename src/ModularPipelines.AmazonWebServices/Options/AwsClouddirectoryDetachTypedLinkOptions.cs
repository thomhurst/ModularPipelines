using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "detach-typed-link")]
public record AwsClouddirectoryDetachTypedLinkOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--typed-link-specifier")] string TypedLinkSpecifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}