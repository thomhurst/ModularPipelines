using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrassv2", "create-deployment")]
public record AwsGreengrassv2CreateDeploymentOptions(
[property: CliOption("--target-arn")] string TargetArn
) : AwsOptions
{
    [CliOption("--deployment-name")]
    public string? DeploymentName { get; set; }

    [CliOption("--components")]
    public IEnumerable<KeyValue>? Components { get; set; }

    [CliOption("--iot-job-configuration")]
    public string? IotJobConfiguration { get; set; }

    [CliOption("--deployment-policies")]
    public string? DeploymentPolicies { get; set; }

    [CliOption("--parent-target-arn")]
    public string? ParentTargetArn { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}