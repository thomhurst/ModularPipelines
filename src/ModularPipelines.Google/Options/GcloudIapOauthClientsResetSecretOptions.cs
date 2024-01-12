using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-clients", "reset-secret")]
public record GcloudIapOauthClientsResetSecretOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Brand
) : GcloudOptions;