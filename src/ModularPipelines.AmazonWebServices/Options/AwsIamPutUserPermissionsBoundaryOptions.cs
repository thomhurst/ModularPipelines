using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "put-user-permissions-boundary")]
public record AwsIamPutUserPermissionsBoundaryOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--permissions-boundary")] string PermissionsBoundary
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}