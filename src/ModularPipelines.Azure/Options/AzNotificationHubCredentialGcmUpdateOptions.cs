using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "credential", "gcm", "update")]
public record AzNotificationHubCredentialGcmUpdateOptions(
[property: CliOption("--google-api-key")] string GoogleApiKey,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;