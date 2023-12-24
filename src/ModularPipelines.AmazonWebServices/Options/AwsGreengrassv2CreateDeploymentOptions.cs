using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrassv2", "create-deployment")]
public record AwsGreengrassv2CreateDeploymentOptions(
[property: CommandSwitch("--target-arn")] string TargetArn
) : AwsOptions
{
    [CommandSwitch("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CommandSwitch("--components")]
    public IEnumerable<KeyValue>? Components { get; set; }

    [CommandSwitch("--iot-job-configuration")]
    public string? IotJobConfiguration { get; set; }

    [CommandSwitch("--deployment-policies")]
    public string? DeploymentPolicies { get; set; }

    [CommandSwitch("--parent-target-arn")]
    public string? ParentTargetArn { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}