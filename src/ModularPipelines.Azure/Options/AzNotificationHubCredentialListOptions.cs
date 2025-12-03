using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notification-hub", "credential", "list")]
public record AzNotificationHubCredentialListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--notification-hub-name")] string NotificationHubName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;