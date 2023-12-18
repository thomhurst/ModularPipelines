using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "test-send")]
public record AzNotificationHubTestSendOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--notification-format")] string NotificationFormat,
[property: CommandSwitch("--notification-hub-name")] string NotificationHubName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--payload")]
    public string? Payload { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }
}

