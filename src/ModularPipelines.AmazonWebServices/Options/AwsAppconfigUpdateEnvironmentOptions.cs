using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "update-environment")]
public record AwsAppconfigUpdateEnvironmentOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--environment-id")] string EnvironmentId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--monitors")]
    public string[]? Monitors { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}