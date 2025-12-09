using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "namespace", "check-availability")]
public record AzNotificationHubNamespaceCheckAvailabilityOptions(
[property: CliOption("--name")] string Name
) : AzOptions;