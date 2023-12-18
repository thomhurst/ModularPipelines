using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "notification-registration", "create")]
public record AzProviderhubNotificationRegistrationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CommandSwitch("--included-events")]
    public string? IncludedEvents { get; set; }

    [CommandSwitch("--message-scope")]
    public string? MessageScope { get; set; }

    [CommandSwitch("--notification-endpoints")]
    public string? NotificationEndpoints { get; set; }

    [CommandSwitch("--notification-mode")]
    public string? NotificationMode { get; set; }
}