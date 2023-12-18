using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "authorization-rule", "list")]
public record AzNotificationHubAuthorizationRuleListOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--notification-hub-name")] string NotificationHubName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }
}