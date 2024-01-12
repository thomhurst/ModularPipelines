using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "create", "github")]
public record GcloudBuildsConnectionsCreateGithubOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--authorizer-token-secret-version")]
    public string? AuthorizerTokenSecretVersion { get; set; }

    [CommandSwitch("--app-installation-id")]
    public string? AppInstallationId { get; set; }
}