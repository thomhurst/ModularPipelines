using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("notification-hub", "credential", "wns", "update")]
public record AzNotificationHubCredentialWnsUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--package-sid")] string PackageSid,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret-key")] string SecretKey
) : AzOptions;