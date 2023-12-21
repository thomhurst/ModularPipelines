using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "notification-registration", "show")]
public record AzProviderhubNotificationRegistrationShowOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--provider-namespace")]
    public string? ProviderNamespace { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}