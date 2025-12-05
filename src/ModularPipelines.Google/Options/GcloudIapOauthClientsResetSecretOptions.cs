using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-clients", "reset-secret")]
public record GcloudIapOauthClientsResetSecretOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Brand
) : GcloudOptions;