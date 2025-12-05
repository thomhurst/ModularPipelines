using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "notification-registration", "create")]
public record AzProviderhubNotificationRegistrationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions
{
    [CliOption("--included-events")]
    public string? IncludedEvents { get; set; }

    [CliOption("--message-scope")]
    public string? MessageScope { get; set; }

    [CliOption("--notification-endpoints")]
    public string? NotificationEndpoints { get; set; }

    [CliOption("--notification-mode")]
    public string? NotificationMode { get; set; }
}