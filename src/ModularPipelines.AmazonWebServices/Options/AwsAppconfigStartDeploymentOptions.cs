using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "start-deployment")]
public record AwsAppconfigStartDeploymentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--environment-id")] string EnvironmentId,
[property: CliOption("--deployment-strategy-id")] string DeploymentStrategyId,
[property: CliOption("--configuration-profile-id")] string ConfigurationProfileId,
[property: CliOption("--configuration-version")] string ConfigurationVersion
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}