using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "test-send")]
public record AzNotificationHubTestSendOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--notification-format")] string NotificationFormat,
[property: CliOption("--notification-hub-name")] string NotificationHubName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--message")]
    public string? Message { get; set; }

    [CliOption("--payload")]
    public string? Payload { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}