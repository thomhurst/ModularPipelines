using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "create-dev-environment")]
public record AwsCodecatalystCreateDevEnvironmentOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--persistent-storage")] string PersistentStorage
) : AwsOptions
{
    [CliOption("--repositories")]
    public string[]? Repositories { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--ides")]
    public string[]? Ides { get; set; }

    [CliOption("--inactivity-timeout-minutes")]
    public int? InactivityTimeoutMinutes { get; set; }

    [CliOption("--vpc-connection-name")]
    public string? VpcConnectionName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}