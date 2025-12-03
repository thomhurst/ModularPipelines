using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-clients", "describe")]
public record GcloudIapOauthClientsDescribeOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Brand
) : GcloudOptions;