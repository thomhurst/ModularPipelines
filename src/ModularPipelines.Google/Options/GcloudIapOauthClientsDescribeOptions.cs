using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-clients", "describe")]
public record GcloudIapOauthClientsDescribeOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Brand
) : GcloudOptions;