using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "wait", "deployment-successful")]
public record AwsOpsworksWaitDeploymentSuccessfulOptions : AwsOptions
{
    [CliOption("--stack-id")]
    public string? StackId { get; set; }

    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--deployment-ids")]
    public string[]? DeploymentIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}