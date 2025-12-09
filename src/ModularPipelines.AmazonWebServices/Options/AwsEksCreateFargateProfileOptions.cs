using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "create-fargate-profile")]
public record AwsEksCreateFargateProfileOptions(
[property: CliOption("--fargate-profile-name")] string FargateProfileName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--pod-execution-role-arn")] string PodExecutionRoleArn
) : AwsOptions
{
    [CliOption("--subnets")]
    public string[]? Subnets { get; set; }

    [CliOption("--selectors")]
    public string[]? Selectors { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}