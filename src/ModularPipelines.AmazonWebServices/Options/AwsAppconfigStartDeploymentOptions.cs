using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "start-deployment")]
public record AwsAppconfigStartDeploymentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--deployment-strategy-id")] string DeploymentStrategyId,
[property: CommandSwitch("--configuration-profile-id")] string ConfigurationProfileId,
[property: CommandSwitch("--configuration-version")] string ConfigurationVersion
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--kms-key-identifier")]
    public string? KmsKeyIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}