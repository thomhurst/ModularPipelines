using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "notification-channel", "delete")]
public record AzGrafanaNotificationChannelDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--notification-channel")] string NotificationChannel
) : AzOptions
{
    [CliOption("--api-key")]
    public string? ApiKey { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}