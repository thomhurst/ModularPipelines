using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-clients", "delete")]
public record GcloudIapOauthClientsDeleteOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Brand
) : GcloudOptions;