using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("providerhub", "notification-registration", "list")]
public record AzProviderhubNotificationRegistrationListOptions(
[property: CliOption("--provider-namespace")] string ProviderNamespace
) : AzOptions;