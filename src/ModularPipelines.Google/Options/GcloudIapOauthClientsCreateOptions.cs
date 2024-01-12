using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-clients", "create")]
public record GcloudIapOauthClientsCreateOptions(
[property: PositionalArgument] string Brand,
[property: CommandSwitch("--display_name")] string DisplayName
) : GcloudOptions;