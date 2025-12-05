using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "get-cluster-session-credentials")]
public record AwsEmrGetClusterSessionCredentialsOptions(
[property: CliOption("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}