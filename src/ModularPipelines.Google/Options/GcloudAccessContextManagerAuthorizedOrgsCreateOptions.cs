using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "authorized-orgs", "create")]
public record GcloudAccessContextManagerAuthorizedOrgsCreateOptions(
[property: CliArgument] string AuthorizedOrgsDesc,
[property: CliArgument] string Policy,
[property: CliOption("--asset_type")] string AssetType,
[property: CliOption("--authorization_direction")] string AuthorizationDirection,
[property: CliOption("--authorization_type")] string AuthorizationType
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--orgs")]
    public string[]? Orgs { get; set; }
}