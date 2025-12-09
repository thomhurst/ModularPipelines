using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("notification-hub", "credential", "wns", "update")]
public record AzNotificationHubCredentialWnsUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--package-sid")] string PackageSid,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-key")] string SecretKey
) : AzOptions;