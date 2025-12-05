using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-access-point-policy-status-for-object-lambda")]
public record AwsS3controlGetAccessPointPolicyStatusForObjectLambdaOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}