using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iap", "oauth-clients", "list")]
public record GcloudIapOauthClientsListOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;