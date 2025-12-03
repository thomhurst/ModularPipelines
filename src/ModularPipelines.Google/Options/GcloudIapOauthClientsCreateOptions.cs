using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iap", "oauth-clients", "create")]
public record GcloudIapOauthClientsCreateOptions(
[property: CliArgument] string Brand,
[property: CliOption("--display_name")] string DisplayName
) : GcloudOptions;