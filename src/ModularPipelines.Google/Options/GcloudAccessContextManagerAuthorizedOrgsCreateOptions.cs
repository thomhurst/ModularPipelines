using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "authorized-orgs", "create")]
public record GcloudAccessContextManagerAuthorizedOrgsCreateOptions(
[property: PositionalArgument] string AuthorizedOrgsDesc,
[property: PositionalArgument] string Policy,
[property: CommandSwitch("--asset_type")] string AssetType,
[property: CommandSwitch("--authorization_direction")] string AuthorizationDirection,
[property: CommandSwitch("--authorization_type")] string AuthorizationType
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--orgs")]
    public string[]? Orgs { get; set; }
}