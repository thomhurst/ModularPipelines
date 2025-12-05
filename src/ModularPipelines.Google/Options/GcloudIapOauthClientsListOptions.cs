using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-clients", "list")]
public record GcloudIapOauthClientsListOptions(
[property: CliArgument] string Name
) : GcloudOptions;