using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "namespace", "check-availability")]
public record AzNotificationHubNamespaceCheckAvailabilityOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;