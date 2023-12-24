using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "update-workteam")]
public record AwsSagemakerUpdateWorkteamOptions(
[property: CommandSwitch("--workteam-name")] string WorkteamName
) : AwsOptions
{
    [CommandSwitch("--member-definitions")]
    public string[]? MemberDefinitions { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--notification-configuration")]
    public string? NotificationConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}