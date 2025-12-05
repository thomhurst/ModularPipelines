using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "detach-policy")]
public record AwsClouddirectoryDetachPolicyOptions(
[property: CliOption("--directory-arn")] string DirectoryArn,
[property: CliOption("--policy-reference")] string PolicyReference,
[property: CliOption("--object-reference")] string ObjectReference
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}