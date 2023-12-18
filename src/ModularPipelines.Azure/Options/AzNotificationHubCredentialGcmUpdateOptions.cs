using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "gcm", "update")]
public record AzNotificationHubCredentialGcmUpdateOptions(
[property: CommandSwitch("--google-api-key")] string GoogleApiKey,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;