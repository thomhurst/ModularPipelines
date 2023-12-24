using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecatalyst", "start-dev-environment")]
public record AwsCodecatalystStartDevEnvironmentOptions(
[property: CommandSwitch("--space-name")] string SpaceName,
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--ides")]
    public string[]? Ides { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--inactivity-timeout-minutes")]
    public int? InactivityTimeoutMinutes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}