using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("providerhub", "notification-registration", "list")]
public record AzProviderhubNotificationRegistrationListOptions(
[property: CommandSwitch("--provider-namespace")] string ProviderNamespace
) : AzOptions;