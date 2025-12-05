using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "start-dev-environment")]
public record AwsCodecatalystStartDevEnvironmentOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--ides")]
    public string[]? Ides { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--inactivity-timeout-minutes")]
    public int? InactivityTimeoutMinutes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}