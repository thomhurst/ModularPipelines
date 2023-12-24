using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-serverless", "start-job-run")]
public record AwsEmrServerlessStartJobRunOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--job-driver")]
    public string? JobDriver { get; set; }

    [CommandSwitch("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--execution-timeout-minutes")]
    public long? ExecutionTimeoutMinutes { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}