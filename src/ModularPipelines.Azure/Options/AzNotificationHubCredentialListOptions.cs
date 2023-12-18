using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "list")]
public record AzNotificationHubCredentialListOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--notification-hub-name")] string NotificationHubName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}

