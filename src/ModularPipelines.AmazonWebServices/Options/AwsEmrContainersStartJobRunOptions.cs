using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-containers", "start-job-run")]
public record AwsEmrContainersStartJobRunOptions(
[property: CliOption("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--release-label")]
    public string? ReleaseLabel { get; set; }

    [CliOption("--job-driver")]
    public string? JobDriver { get; set; }

    [CliOption("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--job-template-id")]
    public string? JobTemplateId { get; set; }

    [CliOption("--job-template-parameters")]
    public IEnumerable<KeyValue>? JobTemplateParameters { get; set; }

    [CliOption("--retry-policy-configuration")]
    public string? RetryPolicyConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}