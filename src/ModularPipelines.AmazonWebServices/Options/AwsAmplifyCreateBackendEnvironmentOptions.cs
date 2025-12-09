using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "create-backend-environment")]
public record AwsAmplifyCreateBackendEnvironmentOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--deployment-artifacts")]
    public string? DeploymentArtifacts { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}