using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-fargate-profile")]
public record AwsEksCreateFargateProfileOptions(
[property: CommandSwitch("--fargate-profile-name")] string FargateProfileName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--pod-execution-role-arn")] string PodExecutionRoleArn
) : AwsOptions
{
    [CommandSwitch("--subnets")]
    public string[]? Subnets { get; set; }

    [CommandSwitch("--selectors")]
    public string[]? Selectors { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}