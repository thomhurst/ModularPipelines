using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "authorization-rule", "create")]
public record AzNotificationHubAuthorizationRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--notification-hub-name")] string NotificationHubName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--rights")]
    public string? Rights { get; set; }
}