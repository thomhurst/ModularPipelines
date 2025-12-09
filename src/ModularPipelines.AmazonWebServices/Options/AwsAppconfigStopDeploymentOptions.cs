using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "stop-deployment")]
public record AwsAppconfigStopDeploymentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--deployment-number")] int DeploymentNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}