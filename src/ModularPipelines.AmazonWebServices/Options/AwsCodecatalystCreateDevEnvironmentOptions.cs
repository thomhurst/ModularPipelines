using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "create-dev-environment")]
public record AwsCodecatalystCreateDevEnvironmentOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--instance-type")] string InstanceType,
[property: CommandSwitch("--persistent-storage")] string PersistentStorage
) : AwsOptions
{
    [CommandSwitch("--repositories")]
    public string[]? Repositories { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--ides")]
    public string[]? Ides { get; set; }

    [CommandSwitch("--inactivity-timeout-minutes")]
    public int? InactivityTimeoutMinutes { get; set; }

    [CommandSwitch("--vpc-connection-name")]
    public string? VpcConnectionName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}