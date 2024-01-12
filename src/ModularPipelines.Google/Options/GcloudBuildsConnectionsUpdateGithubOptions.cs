using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "update", "github")]
public record GcloudBuildsConnectionsUpdateGithubOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--app-installation-id")]
    public string? AppInstallationId { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }
}