using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-workteam")]
public record AwsSagemakerCreateWorkteamOptions(
[property: CommandSwitch("--workteam-name")] string WorkteamName,
[property: CommandSwitch("--member-definitions")] string[] MemberDefinitions,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--workforce-name")]
    public string? WorkforceName { get; set; }

    [CommandSwitch("--notification-configuration")]
    public string? NotificationConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}