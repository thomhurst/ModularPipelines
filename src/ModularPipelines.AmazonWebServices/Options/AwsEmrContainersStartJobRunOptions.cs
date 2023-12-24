using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-containers", "start-job-run")]
public record AwsEmrContainersStartJobRunOptions(
[property: CommandSwitch("--virtual-cluster-id")] string VirtualClusterId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--release-label")]
    public string? ReleaseLabel { get; set; }

    [CommandSwitch("--job-driver")]
    public string? JobDriver { get; set; }

    [CommandSwitch("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--job-template-id")]
    public string? JobTemplateId { get; set; }

    [CommandSwitch("--job-template-parameters")]
    public IEnumerable<KeyValue>? JobTemplateParameters { get; set; }

    [CommandSwitch("--retry-policy-configuration")]
    public string? RetryPolicyConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}