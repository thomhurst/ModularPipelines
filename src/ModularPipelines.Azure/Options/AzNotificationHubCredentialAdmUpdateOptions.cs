using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notification-hub", "credential", "adm", "update")]
public record AzNotificationHubCredentialAdmUpdateOptions(
[property: CliOption("--client-id")] string ClientId,
[property: CliOption("--client-secret")] string ClientSecret,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;