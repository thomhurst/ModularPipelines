using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-clients", "delete")]
public record GcloudIapOauthClientsDeleteOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Brand
) : GcloudOptions;